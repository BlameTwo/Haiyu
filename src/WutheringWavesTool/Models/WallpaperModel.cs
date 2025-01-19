﻿using System.Collections.Generic;
using Microsoft.UI.Xaml.Media.Imaging;

namespace WutheringWavesTool.Models;

public class WallpaperModel
{
    /// <summary>
    /// 低清晰度图像
    /// </summary>
    public BitmapImage Image { get; set; }

    /// <summary>
    /// 文件Md5
    /// </summary>
    public string Md5String { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; }
}
