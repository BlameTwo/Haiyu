namespace Haiyu.Models;

public class WallpaperModel
{
    /// <summary>
    /// 低清晰度图像
    /// </summary>
    public ImageSource Image { get; set; }

    /// <summary>
    /// 文件Md5
    /// </summary>
    public string Md5String { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; }
}
