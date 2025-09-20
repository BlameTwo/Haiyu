﻿//委托
using System.Buffers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
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
        var folder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
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
            Logger.WriteError(ex.Message);
            await this.SetNoneStatusAsync();
        }
    }

    async Task<bool> UpdataDeleteGameResource(GameLauncherSource source)
    {
        var currentVersion = GameLocalConfig.GetConfig(GameLocalSettingName.LocalGameVersion);
        var installFolder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
        var previous = source.ResourceDefault.Config.PatchConfig.Where(x =>
            x.Version == currentVersion
        );

        if (previous != null && previous.Count() > 0)
        {
            if (_downloadCTS.IsCancellationRequested)
            {
                return false;
            }
            var previou = previous.First();
            var cdnUrl = source
                .ResourceDefault.CdnList.Where(x => x.P != 0)
                .OrderBy(x => x.P)
                .First()
                .Url;
            var patch = await GetPatchGameResourceAsync(cdnUrl + previou.IndexFile);
            if (patch == null)
            {
                Logger.WriteError("获取游戏资源失败，已经退出下载");
                return false;
            }
            for (int i = 0; i < patch.DeleteFiles.Count; i++)
            {
                var localFile = $"{installFolder}\\{patch.DeleteFiles[i]}".Replace('/', '\\');
                if (File.Exists(localFile))
                {
                    File.Delete(localFile);
                }
                Logger.WriteInfo($"删除旧文件{System.IO.Path.GetFileName(localFile)}");
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
        return true;
    }

    async Task DownloadComplate(GameLauncherSource source)
    {
        var currentVersion = GameLocalConfig.GetConfig(GameLocalSettingName.LocalGameVersion);
        var installFolder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
        if (string.IsNullOrWhiteSpace(currentVersion))
        {
            await this.GameLocalConfig.SaveConfigAsync(
                GameLocalSettingName.LocalGameVersion,
                source.ResourceDefault.Version
            );
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
            $"{installFolder}\\{this.Config.GameExeName}"
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
        // Fix 修复游戏时旧文件排除
        var localFile = new DirectoryInfo(folder).GetFiles("*", SearchOption.AllDirectories);
        var serverFile = resource.Resource.Select(x => BuildFilePath(folder, x));
        var localFileWhere = localFile.Where(file => !serverFile.Contains(file.FullName)).ToList();
        if (localFileWhere != null && localFileWhere.Any())
        {
            localFileWhere.ForEach(x => File.Delete(x.FullName));
            Logger.WriteInfo(
                    $"删除：删除版本旧文件{string.Join(',', localFileWhere)}"
                );
        }
        await UpdateFileProgress(GameContextActionType.Verify, 0);
        #region 下载逻辑
        try
        {
            for (int j = 0; j < resource.Resource.Count; j++)
            {
                Logger.WriteInfo(
                    $"[{resource.Resource[j].Dest}],当前进度大小[{Math.Round((double)_totalProgressSize, 2)}/{Math.Round((double)_totalfileSize, 2)}]，真剩余资源大小{resource.Resource.Skip(j - 1).Sum(x => x.Size)}"
                );
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
                            Logger.WriteInfo("需要全量下载……");
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
                        else
                        {
                            await UpdateFileProgress(
                                    GameContextActionType.Verify,
                                    resource.Resource[j].Size,
                                    true
                                )
                                .ConfigureAwait(false);
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
                                Logger.WriteInfo($"分片[{i}]需要全量下载……");
                                if (i == resource.Resource[j].ChunkInfos.Count - 1)
                                {
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
                                    await DownloadFileByChunks(
                                        resource.Resource[j].Dest,
                                        filePath,
                                        resource.Resource[j].ChunkInfos[i].Start,
                                        resource.Resource[j].ChunkInfos[i].End,
                                        false
                                    );
                                }
                            }
                            else
                            {
                                await UpdateFileProgress(
                                        GameContextActionType.Verify,
                                        resource.Resource[j].ChunkInfos[i].End
                                            - resource.Resource[j].ChunkInfos[i].Start,
                                        true
                                    )
                                    .ConfigureAwait(false);
                            }
                        }
                    }
                    //await FinalValidation(file, filePath);
                }
                else
                {
                    Logger.WriteInfo($"文件不存在，全量下载");
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
            Logger.WriteError($"退出下载，错误{ex.Message}");
            return;
        }
        catch (OperationCanceledException operEx)
        {
            _downloadState.IsActive = false;
            _downloadCTS.Dispose();
            _downloadCTS = null;
            _isDownload = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            Logger.WriteError($"退出下载，错误{operEx.Message}");
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
            Logger.WriteInfo($"暂停下载");
            return await this._downloadState.PauseAsync();
        }

        return false;
    }

    public async Task<bool> ResumeDownloadAsync()
    {
        if (_downloadState.IsPaused)
        {
            Logger.WriteInfo($"恢复下载");
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
                this._downloadState.IsStop = true;
                await _downloadCTS.CancelAsync().ConfigureAwait(false);
            }
            Interlocked.Exchange(ref _totalProgressSize, 0L);
            Interlocked.Exchange(ref _totalfileSize, 0L);
            Interlocked.Exchange(ref _totalVerifiedBytes, 0L);
            Interlocked.Exchange(ref _totalDownloadedBytes, 0L);
            Logger.WriteInfo($"取消下载");
            return true;
        }
        catch (Exception ex)
        {
            Logger.WriteInfo($"取消下载异常: {ex.Message}");
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
        var currentVersion = GameLocalConfig.GetConfig(GameLocalSettingName.LocalGameVersion);
        var previous = launcher
            .ResourceDefault.Config.PatchConfig.Where(x => x.Version == currentVersion)
            .FirstOrDefault();
        PatchIndexGameResource? patch = null;
        _totalProgressTotal = 0;
        _totalVerifiedBytes = 0;
        _totalDownloadedBytes = 0;

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
            Logger.WriteInfo("本地资源与网络版本不匹配，请直接尝试修复游戏！");
            await CancelDownloadAsync();
            return;
        }
        this._downloadState = new DownloadState(patch);

        _downloadCTS = new CancellationTokenSource();
        bool result = false;
        if (patch.PatchInfos != null)
        {
            _downloadBaseUrl =
                launcher.ResourceDefault.CdnList.Where(x => x.P != 0).OrderBy(x => x.P).First().Url
                + previous.BaseUrl;
            result = await Task.Run(() => DownloadPatcheToResource(folder, patch));
        }
        if (_downloadState.IsStop || _downloadCTS.IsCancellationRequested)
        {
            await SetNoneStatusAsync().ConfigureAwait(false);
            return;
        }
        #region Update Resource
        _downloadBaseUrl =
            launcher.ResourceDefault.CdnList.Where(x => x.P != 0).OrderBy(x => x.P).First().Url
            + launcher.ResourceDefault.ResourcesBasePath;
        _totalfileSize = patch.Resource.Sum(x => x.Size);
        _totalFileTotal = patch.Resource.Count - 1;
        _totalProgressTotal = 0;
        result = await Task.Run(() => UpdateGameToResources(folder, patch));
        await DownloadComplate(launcher);
        #endregion
        #region UpdatePatcher
        await SetNoneStatusAsync().ConfigureAwait(false);
        #endregion
    }

    private async Task<bool> DownloadPatcheToResource(string folder, PatchIndexGameResource patch)
    {
        var patchInfos = patch.PatchInfos.Where(x => x.Dest.EndsWith("krdiff"));
        foreach (var item in patchInfos)
        {
            if (item.Dest.EndsWith("krdiff"))
            {
                var downloadUrl = _downloadBaseUrl + item.Dest;
                var filePath = BuildFilePath(folder, item);
                string? krdiffPath = "";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                krdiffPath = await DownloadFileByKrDiff(item.Dest, filePath);
                if (krdiffPath == null)
                {
                    Logger.WriteError("下载差异文件取消或出现异常");
                    return false;
                }
                await DecompressKrdiffFile(folder, filePath);
            }
        }
        return true;
    }

    private async Task DecompressKrdiffFile(string folder, string? krdiffPath)
    {
        if (krdiffPath == null)
            return;
        DiffDecompressManager manager = new DiffDecompressManager(folder, folder, krdiffPath);
        IProgress<(double, double)> progress = new Progress<(double, double)>();
        (progress as Progress<(double, double)>).ProgressChanged += async (s, e) =>
        {
            if (gameContextOutputDelegate == null)
                return;
            await gameContextOutputDelegate
                .Invoke(
                    this,
                    new GameContextOutputArgs
                    {
                        Type = GameContextActionType.Decompress,
                        CurrentSize = (long)e.Item1,
                        TotalSize = (long)e.Item2,
                        DownloadSpeed = 0,
                        VerifySpeed = 0,
                        RemainingTime = TimeSpan.FromMicroseconds(0),
                        IsAction = _downloadState?.IsActive ?? false,
                        IsPause = _downloadState?.IsPaused ?? false,
                        TipMessage = "正在解压合并资源",
                    }
                )
                .ConfigureAwait(false); // 确保不阻塞下载线程
        };
        await manager.StartAsync(progress);
    }

    private string BuildFilePath(string folder, PatchInfo item)
    {
        var path = Path.Combine(folder, item.Dest.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        return path;
    }

    private async Task<bool> UpdateGameToResources(string folder, PatchIndexGameResource resource)
    {
        _downloadState.IsActive = true;
        _totalProgressTotal = resource.Resource.Sum(x => x.Size);
        await UpdateFileProgress(GameContextActionType.Verify, 0);
        #region 下载逻辑
        try
        {
            for (int i = 0; i < resource.Resource.Count; i++)
            {
                if (resource.Resource[i].Dest.Contains("krdiff"))
                    continue;
                Logger.WriteInfo($"开始处理更新文件{resource.Resource[i].Dest}");
                if (_downloadCTS?.IsCancellationRequested ?? true)
                {
                    this._downloadState.IsActive = false;
                    await SetNoneStatusAsync().ConfigureAwait(false);
                    return false;
                }
                var filePath = BuildFilePath(folder, resource.Resource[i]);
                if (File.Exists(filePath))
                {
                    if (resource.Resource[i].ChunkInfos == null)
                    {
                        var checkResult = await VaildateFullFile(
                            resource.Resource[i].Md5,
                            filePath
                        );
                        if (checkResult)
                        {
                            await DownloadFileByFull(
                                resource.Resource[i].Dest,
                                resource.Resource[i].Size,
                                filePath,
                                new()
                                {
                                    Start = 0,
                                    End = resource.Resource[i].Size - 1,
                                    Md5 = resource.Resource[i].Md5,
                                }
                            );
                        }
                        else
                        {
                            await UpdateFileProgress(
                                    GameContextActionType.Verify,
                                    resource.Resource[i].Size,
                                    true
                                )
                                .ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        for (int c = 0; c < resource.Resource[i].ChunkInfos.Count; c++)
                        {
                            var fileName = System.IO.Path.GetFileName(filePath);
                            var needDownload = await ValidateFileChunks(
                                resource.Resource[i].ChunkInfos[c],
                                filePath
                            );
                            if (needDownload)
                            {
                                if (i == resource.Resource[i].ChunkInfos.Count - 1)
                                {
                                    await DownloadFileByChunks(
                                        resource.Resource[i].Dest,
                                        filePath,
                                        resource.Resource[i].ChunkInfos[c].Start,
                                        resource.Resource[i].ChunkInfos[c].End,
                                        true,
                                        resource.Resource[i].Size
                                    );
                                }
                                else
                                {
                                    await DownloadFileByChunks(
                                        resource.Resource[i].Dest,
                                        filePath,
                                        resource.Resource[i].ChunkInfos[c].Start,
                                        resource.Resource[i].ChunkInfos[c].End,
                                        false
                                    );
                                }
                            }
                            else
                            {
                                await UpdateFileProgress(
                                        GameContextActionType.Verify,
                                        resource.Resource[i].ChunkInfos[c].End
                                            - resource.Resource[i].ChunkInfos[c].Start,
                                        true
                                    )
                                    .ConfigureAwait(false);
                            }
                        }
                    }
                }
                else
                {
                    await DownloadFileByFull(
                        resource.Resource[i].Dest,
                        resource.Resource[i].Size,
                        filePath,
                        new IndexChunkInfo()
                        {
                            Start = 0,
                            End = resource.Resource[i].Size - 1,
                            Md5 = resource.Resource[i].Md5,
                        }
                    );
                }
            }
        }
        catch (IOException ex)
        {
            Debug.WriteLine(ex.Message);
            await this.SetNoneStatusAsync().ConfigureAwait(false);
            return false;
        }
        catch (OperationCanceledException)
        {
            _downloadState.IsActive = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return false;
        }
        catch (Exception ex)
        {
            _downloadState.IsActive = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return false;
        }
        finally
        {
            _downloadCTS.Dispose();
            _downloadCTS = null;
            _isDownload = false;
        }
        #endregion
        _downloadState.IsActive = false;
        return true;
    }

    private async Task<bool> StartPatchDownloadAsync(string folder, PatchIndexGameResource patch)
    {
        _downloadState.IsActive = true;
        await UpdateFileProgress(GameContextActionType.Verify, 0);
        #region 下载逻辑
        try
        {
            for (int i = 0; i < patch.PatchInfos.Count; i++)
            {
                for (int j = 0; j < patch.PatchInfos[i].Entries.Count; j++)
                {
                    Logger.WriteInfo($"开始处理更新文件{patch.PatchInfos[i].Entries[j].Dest}");
                    Debug.WriteLine($"开始处理更新文件{patch.PatchInfos[i].Entries[j].Dest}");
                    if (_downloadCTS?.IsCancellationRequested ?? true)
                    {
                        this._downloadState.IsActive = false;
                        await SetNoneStatusAsync().ConfigureAwait(false);
                        return false;
                    }
                    var filePath = BuildFilePath(folder, patch.PatchInfos[i].Entries[j]);
                    if (File.Exists(filePath))
                    {
                        if (patch.PatchInfos[i].Entries[j].ChunkInfos == null)
                        {
                            var checkResult = await VaildateFullFile(
                                patch.PatchInfos[i].Entries[j].Md5,
                                filePath
                            );
                            if (checkResult)
                            {
                                Debug.WriteLine(
                                    $"文件{patch.PatchInfos[i].Entries[j]}需要重新下载……"
                                );
                                await DownloadFileByFull(
                                    patch.PatchInfos[i].Entries[j].Dest,
                                    patch.PatchInfos[i].Entries[j].Size,
                                    filePath,
                                    new()
                                    {
                                        Start = 0,
                                        End = patch.PatchInfos[i].Entries[j].Size - 1,
                                        Md5 = patch.PatchInfos[i].Entries[j].Md5,
                                    }
                                );
                                Debug.WriteLine(
                                    $"文件{patch.PatchInfos[i].Entries[j]}文件下载完毕"
                                );
                            }
                            else
                            {
                                await UpdateFileProgress(
                                        GameContextActionType.Verify,
                                        patch.PatchInfos[i].Entries[j].Size,
                                        true
                                    )
                                    .ConfigureAwait(false);
                                Debug.WriteLine(
                                    $"文件{patch.PatchInfos[i].Entries[j].Dest}校验完成，无需下载"
                                );
                            }
                        }
                        else
                        {
                            for (
                                int c = 0;
                                c < patch.PatchInfos[i].Entries[j].ChunkInfos.Count;
                                c++
                            )
                            {
                                var fileName = System.IO.Path.GetFileName(filePath);
                                var needDownload = await ValidateFileChunks(
                                    patch.PatchInfos[i].Entries[j].ChunkInfos[c],
                                    filePath
                                );
                                if (needDownload)
                                {
                                    Debug.WriteLine(
                                        $"文件{patch.PatchInfos[i].Entries[j].Dest},分片{c}需要更新"
                                    );
                                    if (i == patch.PatchInfos[i].Entries[j].ChunkInfos.Count - 1)
                                    {
                                        await DownloadFileByChunks(
                                            patch.PatchInfos[i].Entries[j].Dest,
                                            filePath,
                                            patch.PatchInfos[i].Entries[j].ChunkInfos[c].Start,
                                            patch.PatchInfos[i].Entries[j].ChunkInfos[c].End,
                                            true,
                                            patch.PatchInfos[i].Entries[j].Size
                                        );
                                    }
                                    else
                                    {
                                        await DownloadFileByChunks(
                                            patch.PatchInfos[i].Entries[j].Dest,
                                            filePath,
                                            patch.PatchInfos[i].Entries[j].ChunkInfos[c].Start,
                                            patch.PatchInfos[i].Entries[j].ChunkInfos[c].End,
                                            false
                                        );
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine(
                                        $"文件{patch.PatchInfos[i].Entries[j].Dest},分片{c}无需更新"
                                    );
                                    await UpdateFileProgress(
                                            GameContextActionType.Verify,
                                            patch.PatchInfos[i].Entries[j].ChunkInfos[c].End
                                                - patch
                                                    .PatchInfos[i]
                                                    .Entries[j]
                                                    .ChunkInfos[c]
                                                    .Start,
                                            true
                                        )
                                        .ConfigureAwait(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine(
                            $"文件{patch.PatchInfos[i].Entries[j].Dest}文件不存在，需要重新下载"
                        );
                        await DownloadFileByFull(
                            patch.PatchInfos[i].Entries[j].Dest,
                            patch.PatchInfos[i].Entries[j].Size,
                            filePath,
                            new IndexChunkInfo()
                            {
                                Start = 0,
                                End = patch.PatchInfos[i].Entries[j].Size - 1,
                                Md5 = patch.PatchInfos[i].Entries[j].Md5,
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
            return false;
        }
        catch (OperationCanceledException)
        {
            _downloadState.IsActive = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return false;
        }
        catch (Exception ex)
        {
            _downloadState.IsActive = false;
            await GameLocalConfig.SaveConfigAsync(GameLocalSettingName.LocalGameUpdateing, "False");
            await SetNoneStatusAsync().ConfigureAwait(false);
            return false;
        }
        finally
        {
            _downloadCTS.Dispose();
            _downloadCTS = null;
            _isDownload = false;
        }
        #endregion
        _downloadState.IsActive = false;
        return true;
    }

    private async Task<bool> StartDownloadDIffAsync(string folder, PatchIndexGameResource patch)
    {
        return true;
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
        _downloadState?.IsStop = true;
        await _downloadCTS?.CancelAsync();
        _downloadCTS.Dispose();
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
                //if (fs.Length < file.End + 1) // 检查文件长度是否足够
                //{
                //    Debug.WriteLine($"文件长度不足: {fs.Length} < {file.End + 1}");
                //    return true;
                //}
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
                                        accumulatedBytes,
                                        false
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
                    if (accumulatedBytes > 0 && accumulatedBytes < UpdateThreshold)
                    {
                        await UpdateFileProgress(
                                GameContextActionType.Verify,
                                accumulatedBytes,
                                false
                            )
                            .ConfigureAwait(false);
                    }
                    md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                    string hash = BitConverter.ToString(md5.Hash!).Replace("-", "").ToLower();
                    isValid = hash == file.Md5.ToLower();
                    Logger.WriteInfo($"分片校验结果{hash}|{file.Md5}");
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
                            await UpdateFileProgress(
                                    GameContextActionType.Verify,
                                    accumulatedBytes,
                                    false
                                )
                                .ConfigureAwait(false);
                            accumulatedBytes = 0;
                        }
                    }
                    finally
                    {
                        memoryPool.Return(buffer);
                    }
                }
                if (accumulatedBytes < UpdateThreshold)
                {
                    await UpdateFileProgress(GameContextActionType.Verify, accumulatedBytes, false)
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
            return false;
        }
        catch (Exception ex)
        {
            return false;
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
            if (start == 0 && end == -1)
            {
                Logger.WriteError($"文件{filePath}，分片数据错误，start={start},end={end}");
            }
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                _downloadBaseUrl.TrimEnd('/') + "/" + dest.TrimStart('/')
            );
            request.Headers.Range = new RangeHeaderValue(start, end);
            using var response = await HttpClientService.GameDownloadClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                _downloadCTS.Token
            );
            var stream = await response.Content.ReadAsStreamAsync(_downloadCTS.Token);
            if (start < 0 || end < start)
            {
                Logger.WriteError($"分片范围无效: {start}-{end}");
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
        try
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
            )
            {
                if (chunk.Start == 0 && chunk.End == -1)
                {
                    Logger.WriteError(
                        $"文件{filePath}，分片数据错误，start={chunk.Start},end={chunk.End}"
                    );
                    return;
                }
                long currentBytes = 0;
                var url = "";
                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    _downloadBaseUrl.TrimEnd('/') + "/" + dest.TrimStart('/')
                );
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
                    Logger.WriteError($"分片范围无效，start={chunk.Start},end={chunk.End}");
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
                        await UpdateFileProgress(
                                GameContextActionType.Download,
                                accumulatedBytes,
                                true
                            )
                            .ConfigureAwait(false);
                        accumulatedBytes = 0; // 重置累积计数器
                    }
                }
                if (accumulatedBytes > 0 && !isBreak)
                {
                    await UpdateFileProgress(GameContextActionType.Download, accumulatedBytes, true)
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
        catch (Exception ex)
        {
            Logger.WriteError(ex.Message);
        }
    }

    private async Task<string?> DownloadFileByKrDiff(string dest, string filePath)
    {
        try
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
            )
            {
                using var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    _downloadBaseUrl.TrimEnd('/') + "/" + dest.TrimStart('/')
                );
                using var response = await HttpClientService
                    .GameDownloadClient.SendAsync(
                        request,
                        HttpCompletionOption.ResponseHeadersRead,
                        _downloadCTS.Token
                    )
                    .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var stream = await response
                    .Content.ReadAsStreamAsync(_downloadCTS.Token)
                    .ConfigureAwait(false);
                long totalWritten = 0;
                long chunkTotalSize = long.Parse(
                    response.Content.Headers.GetValues("Content-Length").First()
                );
                ;
                var memoryPool = ArrayPool<byte>.Shared;
                fileStream.Seek(0, SeekOrigin.Begin);
                bool isBreak = false;
                _totalfileSize = chunkTotalSize;
                _totalProgressTotal = chunkTotalSize;
                _totalVerifiedBytes = 0;
                _totalDownloadedBytes = 0;
                while (totalWritten < chunkTotalSize)
                {
                    if (_downloadCTS.IsCancellationRequested)
                    {
                        return null;
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
                        await UpdateFileProgress(
                                GameContextActionType.Download,
                                accumulatedBytes,
                                true,
                                "下载差异文件"
                            )
                            .ConfigureAwait(false);
                        accumulatedBytes = 0;
                    }
                }

                if (accumulatedBytes > 0 && !isBreak)
                {
                    await UpdateFileProgress(
                            GameContextActionType.Download,
                            accumulatedBytes,
                            true,
                            "下载差异文件"
                        )
                        .ConfigureAwait(false);
                }
                if (totalWritten != chunkTotalSize)
                {
                    throw new IOException($"分片写入不完整: {totalWritten}/{chunkTotalSize}");
                }
                await fileStream.FlushAsync();
                return filePath;
            }
        }
        catch (Exception ex)
        {
            Logger.WriteError(ex.Message);
            return null;
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

    private async Task UpdateFileProgress(
        GameContextActionType type,
        long fileSize,
        bool isAdd = true,
        string v = null
    )
    {
        if (type == GameContextActionType.Download)
        {
            Interlocked.Add(ref _totalDownloadedBytes, fileSize);
            if (isAdd)
                Interlocked.Add(ref _totalProgressSize, fileSize);
        }
        else if (type == GameContextActionType.Verify)
        {
            if (!isAdd)
                Interlocked.Add(ref _totalVerifiedBytes, fileSize);
            if (isAdd)
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
                    TipMessage = v,
                }
            )
            .ConfigureAwait(false); // 确保不阻塞下载线程
    }

    public TimeSpan RemainingTime
    {
        get
        {
            try
            {
                if (DownloadSpeed <= 0 || _totalDownloadedBytes >= _totalfileSize)
                    return TimeSpan.Zero;

                var remainingBytes = _totalfileSize - _totalProgressSize;
                return TimeSpan.FromSeconds(remainingBytes / DownloadSpeed);
            }
            catch (Exception)
            {
                return TimeSpan.Zero;
            }
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
        if (gameContextOutputDelegate == null)
            return;
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
        Logger.WriteInfo("开始修复游戏");
        var installFolder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
        var launcher = await this.GetGameLauncherSourceAsync();
        await Task.Run(async () => await StartDownloadTaskAsync(installFolder, launcher));
    }
}
