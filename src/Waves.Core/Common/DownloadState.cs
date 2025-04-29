﻿using System.Collections.Concurrent;
using Waves.Core.Models.Downloader;

namespace Waves.Core.Common;

public sealed class DownloadState
{
    public volatile bool _isPaused;
    private long _currentBytes;

    public IndexGameResource Resources { get; }
    public SpeedLimiter SpeedLimiter { get; private set; }
    public bool IsActive { get; set; }
    public CancellationToken CancelToken { get; set; }
    public PauseToken PauseToken => new PauseToken(this);

    public DownloadState(IndexGameResource resources)
    {
        Resources = resources;
        SpeedLimiter = new SpeedLimiter();
        _isPaused = false;
    }

    public DownloadState(PatchIndexGameResource resources)
    {
        Resources = resources;
        SpeedLimiter = new SpeedLimiter();
        _isPaused = false;
    }

    public bool IsPaused => _isPaused;

    public async Task SetSpeedLimitAsync(long bytesPerSecond)
    {
        var newLimiter = new SpeedLimiter();
        await newLimiter.SetBytesPerSecondAsync(bytesPerSecond);
        SpeedLimiter = newLimiter;
    }

    public Task<bool> PauseAsync()
    {
        Volatile.Write(ref _isPaused, true);
        return Task.FromResult(true);
    }

    public Task<bool> ResumeAsync()
    {
        Volatile.Write(ref _isPaused, false);
        return Task.FromResult(true);
    }
}

public readonly struct PauseToken
{
    private readonly DownloadState _state;

    public PauseToken(DownloadState state) => _state = state;

    /// <summary>
    /// 异步等待暂停状态结束（无锁轮询）
    /// </summary>
    public async ValueTask WaitIfPausedAsync()
    {
        while (Volatile.Read(ref _state._isPaused))
        {
            await Task.Delay(100, _state.CancelToken); // 低延迟轮询
        }
    }
}
