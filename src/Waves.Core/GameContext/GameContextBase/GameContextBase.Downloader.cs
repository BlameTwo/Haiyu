//委托
using System.Buffers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Dm.filter;
using SqlSugar;
using Waves.Api.Models;
using Waves.Core.Common;
using Waves.Core.Models;
using Waves.Core.Models.Downloader;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext;

public partial class GameContextBase
{
    #region 常量
    const int MaxBufferSize = 65536; // 64KB缓冲区
    const long UpdateThreshold = 1048576; // 1MB进度更新阈值
    #endregion

    #region 字段和属性
    private string _downloadBaseUrl;
    private long _totalfileSize = 0L;
    private long _totalProgressSize = 0L;
    private long _totalFileTotal = 0L;
    private long _totalProgressTotal = 0L;
    #endregion

    #region DownloadStatus
    private long _totalVerifiedBytes;
    private long _totalDownloadedBytes;
    private DateTime _lastSpeedUpdateTime;
    private double _downloadSpeed;
    private double _verifySpeed;

    private DateTime _lastSpeedTime = DateTime.Now;
    private long _lastSpeedBytes; // 速度计算基准值
    #endregion

    #region 速度属性
    public double DownloadSpeed => _downloadSpeed;
    public double VerifySpeed => _verifySpeed;
    #endregion
    #region DownloadStatus
    private DownloadState? _downloadState;
    #endregion
    #region 公开方法
    public async Task StartDownloadTaskAsync(string folder, GameLauncherSource source)
    {
        if (source == null || string.IsNullOrWhiteSpace(folder))
            return;
        _downloadCTS = new CancellationTokenSource();
        _isDownload = true;
        _totalProgressSize = 0;
        _totalProgressTotal = 0;
        await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.GameLauncherBassFolder, folder);
        await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "True");
        await GetGameResourceAsync(folder, source);
    }
    #endregion


    public async Task UpdateGameAsync()
    {
        _downloadCTS = new CancellationTokenSource();
        var folder = await GameLocalConfig.GetConfigAsync(
            GameLocalSettingName.GameLauncherBassFolder
        );
        var launcher = await this.GetGameLauncherSourceAsync(_downloadCTS.Token);
        if (string.IsNullOrWhiteSpace(folder) || launcher == null)
            return;
        await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "True");

        await UpdateGameResourceAsync(folder, launcher);
    }

    #region 核心下载逻辑
    private async Task GetGameResourceAsync(string folder, GameLauncherSource source)
    {
        try
        {
            var resource = await GetGameResourceAsync(source.ResourceDefault);
            if (resource == null)
                return;

            // 构建下载基础URL
            _downloadBaseUrl =
                source.ResourceDefault.CdnList.Where(x => x.P != 0).OrderBy(x => x.P).First().Url
                + source.ResourceDefault.Config.BaseUrl;

            HttpClientService.BuildClient();
            await InitializeProgress(resource);
            await Task.Run(() => StartDownloadAsync(folder, resource));
            await DownloadComplate(source);

            await SetNoneStatusAsync().ConfigureAwait(false);
        }
        catch (IOException ex)
        {
            Debug.WriteLine(ex.Message);
            await this.SetNoneStatusAsync();
        }
    }

    async Task DownloadComplate(GameLauncherSource source)
    {
        var currentVersion = await GameLocalConfig.GetConfigAsync(
            GameLocalSettingName.LocalGameVersion
        );
        var installFolder = await GameLocalConfig.GetConfigAsync(
            GameLocalSettingName.GameLauncherBassFolder
        );
        if (string.IsNullOrWhiteSpace(currentVersion))
        {
            //检查上次版本，清除缓存文件
            await this.GameLocalConfig.SaveConfigAsync(
                GameLocalSettingName.LocalGameVersion,
                source.ResourceDefault.Version
            );
        }
        else
        {
            var previous = source.ResourceDefault.Config.PatchConfig.Where(x =>
                x.Version == currentVersion
            );

            if (previous != null && previous.Count() > 0)
            {
                var previou = previous.First();
                var cdnUrl = source
                    .ResourceDefault.CdnList.Where(x => x.P != 0)
                    .OrderBy(x => x.P)
                    .First()
                    .Url;
                var patch = await GetPatchGameResourceAsync(cdnUrl + previou.IndexFile);
                if (patch == null)
                {
                    //无网络
                    return;
                }
                for (int i = 0; i < patch.DeleteFiles.Count; i++)
                {
                    var localFile = $"{installFolder}\\{patch.DeleteFiles[i]}".Replace('/', '\\');
                    if (File.Exists(localFile))
                    {
                        File.Delete(localFile);
                    }
                    this.gameContextOutputDelegate?.Invoke(
                            this,
                            new GameContextOutputArgs()
                            {
                                Type = GameContextActionType.DeleteFile,
                                FileTotal = patch.DeleteFiles.Count,
                                CurrentFile = i,
                                DeleteString =
                                    i != patch.DeleteFiles.Count
                                        ? $"正在删除旧文件{System.IO.Path.GetFileName(localFile)}"
                                        : "稍微等一下，马上就好",
                            }
                        )
                        .ConfigureAwait(false);
                }
            }
        }
        await this.GameLocalConfig.SaveConfigAsync(
            GameLocalSettingName.LocalGameVersion,
            source.ResourceDefault.Version
        );
        await this.GameLocalConfig.SaveConfigAsync(
            GameLocalSettingName.LocalGameUpdateing,
            "False"
        );
        await this.GameLocalConfig.SaveConfigAsync(
            GameLocalSettingName.GameLauncherBassProgram,
            $"{installFolder}\\Wuthering Waves.exe"
        );
        if (gameContextOutputDelegate == null)
        {
            return;
        }
        await this
            .gameContextOutputDelegate.Invoke(
                this,
                new GameContextOutputArgs() { Type = GameContextActionType.None }
            )
            .ConfigureAwait(false);
    }

    private async Task StartDownloadAsync(string folder, IndexGameResource resource)
    {
        _downloadState.IsActive = true;
        await UpdateFileProgress(GameContextActionType.Verify, 0);
        #region 下载逻辑
        try
        {
            for (int j = 0; j < resource.Resource.Count; j++)
            {
                //this._totalProgressTotal = j + 1;
                Debug.WriteLine($"开始处理文件[{resource.Resource[j]}/{resource.Resource.Count}]");
                if (_downloadCTS?.IsCancellationRequested ?? true)
                {
                    this._downloadState.IsActive = false;
                    await SetNoneStatusAsync().ConfigureAwait(false);
                    return;
                }
                var filePath = BuildFilePath(folder, resource.Resource[j]);
                if (File.Exists(filePath))
                {
                    if (resource.Resource[j].ChunkInfos == null)
                    {
                        var checkResult = await VaildateFullFile(
                            resource.Resource[j].Md5,
                            filePath
                        );
                        if (checkResult)
                        {
                            await DownloadFileByFull(
                                resource.Resource[j].Dest,
                                resource.Resource[j].Size,
                                filePath,
                                new()
                                {
                                    Start = 0,
                                    End = resource.Resource[j].Size - 1,
                                    Md5 = resource.Resource[j].Md5,
                                }
                            );
                            //await FinalValidation(file, filePath);
                        }
                    }
                    else
                    {
                        var fileName = System.IO.Path.GetFileName(filePath);
                        for (int i = 0; i < resource.Resource[j].ChunkInfos.Count; i++)
                        {
                            var needDownload = await ValidateFileChunks(
                                resource.Resource[j].ChunkInfos[i],
                                filePath
                            );
                            if (needDownload)
                            {
                                if (i == resource.Resource[j].ChunkInfos.Count - 1)
                                {
                                    HttpClientService.BuildClient();
                                    await DownloadFileByChunks(
                                        resource.Resource[j].Dest,
                                        filePath,
                                        resource.Resource[j].ChunkInfos[i].Start,
                                        resource.Resource[j].ChunkInfos[i].End,
                                        true,
                                        resource.Resource[j].Size
                                    );
                                }
                                else
                                {
                                    HttpClientService.BuildClient();
                                    await DownloadFileByChunks(
                                        resource.Resource[j].Dest,
                                        filePath,
                                        resource.Resource[j].ChunkInfos[i].Start,
                                        resource.Resource[j].ChunkInfos[i].End,
                                        false
                                    );
                                }
                            }
                        }
                    }
                    //await FinalValidation(file, filePath);
                }
                else
                {
                    await DownloadFileByFull(
                        resource.Resource[j].Dest,
                        resource.Resource[j].Size,
                        filePath,
                        new IndexChunkInfo()
                        {
                            Start = 0,
                            End = resource.Resource[j].Size - 1,
                            Md5 = resource.Resource[j].Md5,
                        }
                    );
                    //await FinalValidation(file, filePath);
                }
            }
        }
        catch (IOException ex)
        {
            _downloadState.IsActive = false;
            _downloadCTS.Dispose();
            _downloadCTS = null;
            _isDownload = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return;
        }
        catch (OperationCanceledException)
        {
            _downloadState.IsActive = false;
            _downloadCTS.Dispose();
            _downloadCTS = null;
            _isDownload = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return;
        }
        #endregion
        _downloadCTS.Dispose();
        _downloadCTS = null;
        _isDownload = false;
        _downloadState.IsActive = false;
    }

    public async Task<bool> PauseDownloadAsync()
    {
        if (this._downloadState.IsActive)
        {
            return await this._downloadState.PauseAsync();
        }

        return false;
    }

    public async Task<bool> ResumeDownloadAsync()
    {
        if (_downloadState.IsPaused)
        {
            _lastSpeedTime = DateTime.Now;
            return await _downloadState.ResumeAsync();
        }
        return false;
    }

    public async Task<bool> StopDownloadAsync()
    {
        try
        {
            if (_downloadCTS != null && !_downloadCTS.IsCancellationRequested)
            {
                await _downloadCTS.CancelAsync().ConfigureAwait(false);
            }
            Interlocked.Exchange(ref _totalProgressSize, 0L);
            Interlocked.Exchange(ref _totalfileSize, 0L);
            Interlocked.Exchange(ref _totalVerifiedBytes, 0L);
            Interlocked.Exchange(ref _totalDownloadedBytes, 0L);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"取消下载异常: {ex.Message}");
            return false;
        }
        finally
        {
            this._isDownload = false;
            _downloadCTS?.Dispose();
        }
    }
    #endregion

    async Task UpdateGameResourceAsync(string folder, GameLauncherSource launcher)
    {
        var currentVersion = await GameLocalConfig.GetConfigAsync(
            GameLocalSettingName.LocalGameVersion
        );
        var previous = launcher
            .ResourceDefault.Config.PatchConfig.Where(x => x.Version == currentVersion)
            .FirstOrDefault();
        PatchIndexGameResource? patch = null;
        if (previous != null)
        {
            var cdnUrl =
                launcher
                    .ResourceDefault.CdnList.Where(x => x.P != 0)
                    .OrderBy(x => x.P)
                    .FirstOrDefault() ?? null;
            if (cdnUrl == null)
            {
                await CancelDownloadAsync();
                return;
            }
            patch = await GetPatchGameResourceAsync(cdnUrl.Url + previous.IndexFile);
        }
        else
        {
            await CancelDownloadAsync();
            return;
        }

        _totalfileSize = patch.PatchInfos.Sum(x => x.Entries.Sum(x => x.Size));
        _totalFileTotal = patch.Resource.Count - 1;
        _totalProgressTotal = 0;
        this._downloadState = new DownloadState(patch);
        _downloadBaseUrl =
            launcher.ResourceDefault.CdnList.Where(x => x.P != 0).OrderBy(x => x.P).First().Url
            + launcher.ResourceDefault.Config.BaseUrl;
        await gameContextOutputDelegate?.Invoke(
            this,
            new GameContextOutputArgs
            {
                CurrentSize = 0,
                TotalSize = patch.Resource.Sum(x => x.Size),
                Type = GameContextActionType.Download,
            }
        );
        await Task.Run(() => StartPathDownloadAsync(folder, patch));
        await DownloadComplate(launcher);
        await SetNoneStatusAsync().ConfigureAwait(false);
    }

    private async Task StartPathDownloadAsync(string folder, PatchIndexGameResource resource)
    {
        _downloadState.IsActive = true;
        await UpdateFileProgress(GameContextActionType.Verify, 0);
        #region 下载逻辑
        try
        {
            for (int j = 0; j < resource.PatchInfos.Count; j++)
            {
                for (int i = 0; i < resource.PatchInfos[j].Entries.Count; i++)
                {
                    Debug.WriteLine($"开始处理更新文件{resource.PatchInfos[j].Entries[i].Dest}");
                    if (_downloadCTS?.IsCancellationRequested ?? true)
                    {
                        this._downloadState.IsActive = false;
                        await SetNoneStatusAsync().ConfigureAwait(false);
                        return;
                    }
                    var filePath = BuildFilePath(folder, resource.PatchInfos[j].Entries[i]);
                    if (File.Exists(filePath))
                    {
                        if (resource.PatchInfos[j].Entries[i].ChunkInfos == null)
                        {
                            var checkResult = await VaildateFullFile(
                                resource.PatchInfos[j].Entries[i].Md5,
                                filePath
                            );
                            if (checkResult)
                            {
                                await DownloadFileByFull(
                                    resource.PatchInfos[j].Entries[i].Dest,
                                    resource.PatchInfos[j].Entries[i].Size,
                                    filePath,
                                    new()
                                    {
                                        Start = 0,
                                        End = resource.PatchInfos[j].Entries[i].Size - 1,
                                        Md5 = resource.PatchInfos[j].Entries[i].Md5,
                                    }
                                );
                            }
                        }
                        else
                        {
                            for (
                                int c = 0;
                                c < resource.PatchInfos[j].Entries[i].ChunkInfos.Count;
                                c++
                            )
                            {
                                var fileName = System.IO.Path.GetFileName(filePath);
                                var needDownload = await ValidateFileChunks(
                                    resource.PatchInfos[j].Entries[i].ChunkInfos[c],
                                    filePath
                                );
                                if (needDownload)
                                {
                                    if (i == resource.PatchInfos[j].Entries[i].ChunkInfos.Count - 1)
                                    {
                                        HttpClientService.BuildClient();
                                        await DownloadFileByChunks(
                                            resource.Resource[j].Dest,
                                            filePath,
                                            resource.Resource[j].ChunkInfos[c].Start,
                                            resource.Resource[j].ChunkInfos[c].End,
                                            true,
                                            resource.Resource[j].Size
                                        );
                                    }
                                    else
                                    {
                                        HttpClientService.BuildClient();
                                        await DownloadFileByChunks(
                                            resource.Resource[j].Dest,
                                            filePath,
                                            resource.PatchInfos[j].Entries[i].ChunkInfos[c].Start,
                                            resource.PatchInfos[j].Entries[i].ChunkInfos[c].End,
                                            false
                                        );
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        await DownloadFileByFull(
                            resource.Resource[j].Dest,
                            resource.Resource[j].Size,
                            filePath,
                            new IndexChunkInfo()
                            {
                                Start = 0,
                                End = resource.Resource[j].Size - 1,
                                Md5 = resource.Resource[j].Md5,
                            }
                        );
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Debug.WriteLine(ex.Message);
            await this.SetNoneStatusAsync().ConfigureAwait(false);
            return;
        }
        catch (OperationCanceledException)
        {
            _downloadState.IsActive = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return;
        }
        catch (Exception ex)
        {
            _downloadState.IsActive = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return;
        }
        finally
        {
            _downloadCTS.Dispose();
            _downloadCTS = null;
            _isDownload = false;
        }
        #endregion
        _downloadState.IsActive = false;
    }

    async Task CancelDownloadAsync()
    {
        if (this.gameContextOutputDelegate != null)
        {
            await this
                .gameContextOutputDelegate.Invoke(
                    this,
                    new GameContextOutputArgs()
                    {
                        Type = GameContextActionType.None,
                        ErrorString = "未找到版本更新信息！",
                    }
                )
                .ConfigureAwait(false);
        }
        this._isDownload = false;
        await _downloadCTS?.CancelAsync();
        _downloadCTS.Dispose();
        _downloadCTS = null;
        return;
    }

    #region 校验逻辑
    /// <summary>
    /// 校验
    /// </summary>
    /// <param name="file"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private async Task<bool> ValidateFileChunks(IndexChunkInfo file, string filePath)
    {
        using (
            var fs = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                262144,
                true
            )
        )
        {
            try
            {
                if (fs.Length < file.End + 1) // 检查文件长度是否足够
                {
                    Debug.WriteLine($"文件长度不足: {fs.Length} < {file.End + 1}");
                    return true;
                }
                var memoryPool = ArrayPool<byte>.Shared;
                long offset = file.Start;
                long remaining = file.End - file.Start + 1;
                bool isValid = true;
                fs.Seek(offset, SeekOrigin.Begin);
                using (var md5 = MD5.Create())
                {
                    long accumulatedBytes = 0L;
                    bool isBreak = false;
                    while (remaining > 0 && isValid)
                    {
                        await this._downloadState.PauseToken.WaitIfPausedAsync();
                        var buffer = memoryPool.Rent(MaxBufferSize);
                        try
                        {
                            if (_downloadCTS.IsCancellationRequested)
                            {
                                throw new OperationCanceledException();
                            }
                            int bytesRead = await fs.ReadAsync(
                                    buffer,
                                    0,
                                    MaxBufferSize,
                                    _downloadCTS.Token
                                )
                                .ConfigureAwait(false);
                            if (bytesRead == 0)
                            {
                                break;
                            }
                            md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                            remaining -= bytesRead;
                            accumulatedBytes += bytesRead;
                            if (accumulatedBytes >= UpdateThreshold)
                            {
                                await UpdateFileProgress(
                                        GameContextActionType.Verify,
                                        accumulatedBytes
                                    )
                                    .ConfigureAwait(false);
                                accumulatedBytes = 0;
                            }
                        }
                        catch (IOException ex) { }
                        finally
                        {
                            memoryPool.Return(buffer);
                        }
                    }
                    if (accumulatedBytes > 0 && accumulatedBytes < UpdateThreshold && !isBreak)
                    {
                        await UpdateFileProgress(GameContextActionType.Verify, accumulatedBytes)
                            .ConfigureAwait(false);
                    }
                    md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                    string hash = BitConverter.ToString(md5.Hash!).Replace("-", "").ToLower();
                    isValid = hash == file.Md5.ToLower();
                    return !isValid;
                }
            }
            catch (IOException ex)
            {
                return false;
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException();
            }
        }
    }

    private async Task<bool> VaildateFullFile(string md5Value, string filePath)
    {
        const int bufferSize = 262144; // 80KB缓冲区
        using var md5 = MD5.Create();
        var memoryPool = ArrayPool<byte>.Shared;
        const long UpdateThreshold = 1048576; // 1MB进度更新阈值
        try
        {
            using (
                var fs = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: bufferSize,
                    true
                )
            )
            {
                long accumulatedBytes = 0L;
                bool isBreak = false;
                while (true)
                {
                    if (_downloadCTS.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }
                    //暂停锁
                    await this._downloadState.PauseToken.WaitIfPausedAsync();
                    byte[] buffer = memoryPool.Rent(bufferSize);
                    try
                    {
                        int bytesRead = await fs.ReadAsync(
                                buffer.AsMemory(0, bufferSize),
                                _downloadCTS.Token
                            )
                            .ConfigureAwait(false);
                        if (bytesRead == 0)
                        {
                            isBreak = true;
                            break;
                        }
                        md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                        accumulatedBytes += bytesRead; // 添加此行以累加字节数
                        if (accumulatedBytes >= UpdateThreshold)
                        {
                            await UpdateFileProgress(GameContextActionType.Verify, accumulatedBytes)
                                .ConfigureAwait(false);
                            accumulatedBytes = 0;
                        }
                    }
                    finally
                    {
                        memoryPool.Return(buffer);
                    }
                }
                if (accumulatedBytes < UpdateThreshold && !isBreak)
                {
                    await UpdateFileProgress(GameContextActionType.Verify, accumulatedBytes)
                        .ConfigureAwait(false);
                }
            }

            md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            string hash = BitConverter.ToString(md5.Hash!).Replace("-", "").ToLower();
            return !(hash == md5Value);
        }
        catch (OperationCanceledException)
        {
            throw new OperationCanceledException();
        }
        catch (IOException ex)
        {
            return true;
        }
        catch (Exception ex)
        {
            return true;
        }
    }

    #endregion

    #region 下载逻辑
    private async Task DownloadFileByChunks(
        string dest,
        string filePath,
        long start,
        long end,
        bool isLast = false,
        long allSize = 0L
    )
    {
        using (
            var fileStream = new FileStream(
                filePath,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.Read,
                262144,
                true
            )
        )
        {
            long accumulatedBytes = 0;
            using var request = new HttpRequestMessage(HttpMethod.Get, _downloadBaseUrl + dest);
            request.Headers.Range = new RangeHeaderValue(start, end);
            using var response = await HttpClientService.GameDownloadClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                _downloadCTS.Token
            );
            var stream = await response.Content.ReadAsStreamAsync(_downloadCTS.Token);
            if (start < 0 || end < start)
            {
                throw new ArgumentException($"分片范围无效: {start}-{end}");
            }

            long totalWritten = 0;
            long chunkTotalSize = end - start + 1;
            var memoryPool = ArrayPool<byte>.Shared;
            fileStream.Seek(start, SeekOrigin.Begin);
            bool isBreak = false;
            while (totalWritten < chunkTotalSize)
            {
                if (_downloadCTS.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                await _downloadState.PauseToken.WaitIfPausedAsync().ConfigureAwait(false); // 暂停检查也异步化
                int bytesToRead = (int)Math.Min(MaxBufferSize, chunkTotalSize - totalWritten);
                byte[] buffer = ArrayPool<byte>.Shared.Rent(bytesToRead);
                try
                {
                    int bytesRead = await stream
                        .ReadAsync(buffer.AsMemory(0, bytesToRead), _downloadCTS.Token)
                        .ConfigureAwait(false);
                    if (bytesRead == 0)
                    {
                        isBreak = true;
                        break;
                    }
                    await _downloadState.SpeedLimiter.LimitAsync(bytesRead).ConfigureAwait(false);
                    await fileStream
                        .WriteAsync(buffer.AsMemory(0, bytesRead), _downloadCTS.Token)
                        .ConfigureAwait(false);
                    totalWritten += bytesRead;
                    accumulatedBytes += bytesRead;
                    if (accumulatedBytes >= UpdateThreshold)
                    {
                        await UpdateFileProgress(GameContextActionType.Download, accumulatedBytes)
                            .ConfigureAwait(false);
                        accumulatedBytes = 0;
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
            if (accumulatedBytes > 0 && !isBreak)
            {
                await UpdateFileProgress(GameContextActionType.Download, accumulatedBytes)
                    .ConfigureAwait(false);
            }
            if (isLast)
                fileStream.SetLength(allSize);
            await fileStream.FlushAsync(_downloadCTS.Token);
            await fileStream.FlushAsync();
            stream.Close();
            await stream.DisposeAsync();
        }
    }

    public async Task SetNoneStatusAsync()
    {
        await this.gameContextOutputDelegate?.Invoke(
            this,
            new GameContextOutputArgs()
            {
                Type = GameContextActionType.None,
                CurrentSize = _totalProgressSize,
                TotalSize = _totalfileSize,
                DownloadSpeed = _downloadSpeed,
                VerifySpeed = VerifySpeed,
                RemainingTime = this.RemainingTime,
            }
        );
    }

    public async Task SetSpeedLimitAsync(long bytesPerSecond)
    {
        await _downloadState.SetSpeedLimitAsync(bytesPerSecond);
    }

    private async Task DownloadFileByFull(
        string dest,
        long size,
        string filePath,
        IndexChunkInfo chunk
    )
    {
        long accumulatedBytes = 0;

        using (
            var fileStream = new FileStream(
                filePath,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.None,
                262144,
                true
            )
        ) // 明确启用异步IO
        {
            long currentBytes = 0;
            using var request = new HttpRequestMessage(HttpMethod.Get, _downloadBaseUrl + dest);
            request.Headers.Range = new RangeHeaderValue(chunk.Start, chunk.End);

            using var response = await HttpClientService
                .GameDownloadClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    _downloadCTS.Token
                )
                .ConfigureAwait(false); // 非UI上下文切换

            response.EnsureSuccessStatusCode();
            var stream = await response
                .Content.ReadAsStreamAsync(_downloadCTS.Token)
                .ConfigureAwait(false);
            if (chunk.Start < 0 || chunk.End < chunk.Start)
            {
                throw new ArgumentException($"分片范围无效: {chunk.Start}-{chunk.End}");
            }

            long totalWritten = 0;
            long chunkTotalSize = chunk.End - chunk.Start + 1;
            var memoryPool = ArrayPool<byte>.Shared;

            fileStream.Seek(chunk.Start, SeekOrigin.Begin);
            bool isBreak = false;
            while (totalWritten < chunkTotalSize)
            {
                if (_downloadCTS.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                await _downloadState.PauseToken.WaitIfPausedAsync().ConfigureAwait(false); // 暂停检查也异步化

                int bytesToRead = (int)Math.Min(MaxBufferSize, chunkTotalSize - totalWritten);
                byte[] buffer = memoryPool.Rent(bytesToRead);
                int bytesRead = await stream
                    .ReadAsync(buffer.AsMemory(0, bytesToRead), _downloadCTS.Token)
                    .ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    isBreak = true;
                }
                await _downloadState.SpeedLimiter.LimitAsync(bytesRead).ConfigureAwait(false);
                await fileStream
                    .WriteAsync(buffer.AsMemory(0, bytesRead), _downloadCTS.Token)
                    .ConfigureAwait(false);
                totalWritten += bytesRead;
                accumulatedBytes += bytesRead;
                if (accumulatedBytes >= UpdateThreshold)
                {
                    await UpdateFileProgress(GameContextActionType.Download, accumulatedBytes)
                        .ConfigureAwait(false);
                    accumulatedBytes = 0;
                }
            }
            if (accumulatedBytes > 0 && !isBreak)
            {
                await UpdateFileProgress(GameContextActionType.Download, accumulatedBytes)
                    .ConfigureAwait(false);
            }
            if (totalWritten != chunkTotalSize)
            {
                throw new IOException($"分片写入不完整: {totalWritten}/{chunkTotalSize}");
            }
            fileStream.SetLength(size);
            await fileStream.FlushAsync();
        }
    }
    #endregion

    #region 辅助方法

    private async Task FinalValidation(IndexResource file, string filePath)
    {
        try
        {
            using (
                var fs = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    262144,
                    true
                )
            )
            {
                var fullHash = await ComputeFullHash(fs);
                if (fullHash.ToLower() != file.Md5.ToLower()) { }
                else
                {
                    await fs.FlushAsync();
                }
            }
        }
        catch (Exception ex) { }
    }

    private async Task UpdateFileProgress(GameContextActionType type, long fileSize)
    {
        if (type == GameContextActionType.Download)
        {
            Interlocked.Add(ref _totalDownloadedBytes, fileSize);
            Interlocked.Add(ref _totalProgressSize, fileSize);
        }
        else if (type == GameContextActionType.Verify)
        {
            Interlocked.Add(ref _totalVerifiedBytes, fileSize);
            Interlocked.Add(ref _totalProgressSize, fileSize);
        }
        var elapsed = (DateTime.Now - _lastSpeedUpdateTime).TotalSeconds;
        if (elapsed >= 1)
        {
            _downloadSpeed = _totalDownloadedBytes / elapsed;
            _verifySpeed = _totalVerifiedBytes / elapsed;
            // 重置计数器和时间
            Interlocked.Exchange(ref _totalDownloadedBytes, 0);
            Interlocked.Exchange(ref _totalVerifiedBytes, 0);
            var currentBytes = Interlocked.Read(ref _totalDownloadedBytes);
            _lastSpeedBytes = currentBytes;
            _lastSpeedUpdateTime = DateTime.Now;
        }

        if (gameContextOutputDelegate == null)
            return;
        await gameContextOutputDelegate
            .Invoke(
                this,
                new GameContextOutputArgs
                {
                    Type = type,
                    CurrentSize = _totalProgressSize,
                    TotalSize = _totalfileSize,
                    //CurrentSize = _totalProgressTotal,
                    //TotalSize = _totalFileTotal,
                    DownloadSpeed = _downloadSpeed,
                    VerifySpeed = _verifySpeed,
                    RemainingTime = RemainingTime,
                    IsAction = _downloadState?.IsActive ?? false,
                    IsPause = _downloadState?.IsPaused ?? false,
                }
            )
            .ConfigureAwait(false); // 确保不阻塞下载线程
    }

    public TimeSpan RemainingTime
    {
        get
        {
            if (DownloadSpeed <= 0 || _totalDownloadedBytes >= _totalfileSize)
                return TimeSpan.Zero;

            var remainingBytes = _totalfileSize - _totalProgressSize;
            return TimeSpan.FromSeconds(remainingBytes / DownloadSpeed);
        }
    }

    #endregion

    #region 公共辅助方法
    private static async Task<string> ComputeFullHash(Stream stream)
    {
        using var md5 = MD5.Create();
        var hashBytes = await md5.ComputeHashAsync(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "");
    }

    private string BuildFilePath(string folder, IndexResource file)
    {
        var path = Path.Combine(folder, file.Dest.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        return path;
    }

    private async Task InitializeProgress(IndexGameResource resource)
    {
        _totalfileSize = resource.Resource.Sum(x => x.Size);
        _totalFileTotal = resource.Resource.Count - 1;
        _totalProgressTotal = 0;
        this._downloadState = new DownloadState(resource);
        await gameContextOutputDelegate?.Invoke(
            this,
            new GameContextOutputArgs
            {
                CurrentSize = 0,
                TotalSize = resource.Resource.Sum(x => x.Size),
                Type = GameContextActionType.Download,
            }
        );
    }
    #endregion

    public async Task RepirGameAsync()
    {
        var installFolder = await GameLocalConfig.GetConfigAsync(
            GameLocalSettingName.GameLauncherBassFolder
        );
        var launcher = await this.GetGameLauncherSourceAsync();
        await Task.Run(async () => await StartDownloadTaskAsync(installFolder, launcher));
    }
}
