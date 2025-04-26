﻿namespace Waves.Core.Models;

public class GameContextStatus
{
    /// <summary>
    /// 游戏是否已经安装
    /// </summary>
    public bool IsGameExists { get; internal set; }

    /// <summary>
    /// 游戏是否下载完毕
    /// </summary>
    public bool IsGameInstalled { get; internal set; }

    /// <summary>
    /// 游戏是否更新中
    /// </summary>
    public bool IsUpdateing { get; internal set; }

    /// <summary>
    /// 显示版本
    /// </summary>
    public string DisplayVersion { get; set; }

    /// <summary>
    /// 是否显示启动按钮
    /// </summary>
    public bool IsLauncher { get; set; }

    /// <summary>
    /// 游戏是否更新
    /// </summary>
    public bool IsUpdate { get; internal set; }

    /// <summary>
    /// 更新/下载 暂停
    /// </summary>
    public bool IsPause { get; internal set; }

    /// <summary>
    /// 更新/下载 活动
    /// </summary>
    public bool IsAction { get; internal set; }

    /// <summary>
    /// 游戏中
    /// </summary>
    public bool Gameing { get; internal set; }
}
