﻿using Waves.Api.Models;
using Waves.Api.Models.Launcher;
using Waves.Core.Contracts;
using Waves.Core.Models;
using Waves.Core.Models.Downloader;

namespace Waves.Core.GameContext;

/// <summary>
/// 游戏核心管理
/// </summary>
public interface IGameContext
{
    public IHttpClientService HttpClientService { get; set; }

    public Task InitAsync();
    public string ContextName { get; }
    event GameContextOutputDelegate GameContextOutput;
    event GameContextProdOutputDelegate GameContextProdOutput;
    public string GamerConfigPath { get; internal set; }
    GameLocalConfig GameLocalConfig { get; }

    Task<FileVersion> GetLocalDLSSAsync();
    Task<FileVersion> GetLocalDLSSGenerateAsync();
    Task<FileVersion> GetLocalXeSSGenerateAsync();
    public Type ContextType { get; }

    public TimeSpan GetGameTime();

    public Task RepirGameAsync();

    #region Launcher
    Task<GameLauncherSource?> GetGameLauncherSourceAsync(CancellationToken token = default);

    Task<GameLauncherStarter?> GetLauncherStarterAsync(CancellationToken token = default);
    #endregion

    #region Core
    Task<GameContextStatus> GetGameContextStatusAsync(CancellationToken token = default);
    #endregion

    #region Downloader
    Task<IndexGameResource> GetGameResourceAsync(
        ResourceDefault ResourceDefault,
        CancellationToken token = default
    );
    Task<PatchIndexGameResource?> GetPatchGameResourceAsync(
        string url,
        CancellationToken token = default
    );

    /// <summary>
    /// 开始下载
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    Task StartDownloadTaskAsync(string folder, GameLauncherSource? source);

    /// <summary>
    /// 恢复任务
    /// </summary>
    /// <returns></returns>
    Task<bool> ResumeDownloadAsync();

    /// <summary>
    /// 取消下载
    /// </summary>
    /// <returns></returns>
    Task<bool> StopDownloadAsync();

    /// <summary>
    /// 开始任务
    /// </summary>
    /// <returns></returns>
    Task<bool> PauseDownloadAsync();

    /// <summary>
    /// 设置限速
    /// </summary>
    /// <param name="bytesPerSecond"></param>
    /// <returns></returns>
    Task SetSpeedLimitAsync(long bytesPerSecond);

    /// <summary>
    /// 获得游戏登陆的OAuth的代码
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<List<KRSDKLauncherCache>?> GetLocalGameOAuthAsync(CancellationToken token);

    Task StartGameAsync();
    Task UpdateGameAsync();
    Task StopGameAsync();
    Task DeleteResourceAsync();
    #endregion

    Task<LIndex?> GetDefaultLauncherValue(CancellationToken token = default);

    Task<LauncherBackgroundData?> GetLauncherBackgroundDataAsync(string backgroundCode, CancellationToken token = default);
}
