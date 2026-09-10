using Haiyu.Models.Enums;

namespace Haiyu.Models.Options;

/// <summary>
/// 九宫格定位透明窗口配置。尺寸与边距使用 DIP。
/// </summary>
public sealed class PostionTransparentWindowOption : WindowManagerOption
{
    [SetsRequiredMembers]
    public PostionTransparentWindowOption()
    {
        WindowConfig = new WindowsOption();
    }

    public PostionType Postion { get; init; } = PostionType.LeftTop;

    public double Width { get; init; } = 800;

    public double Height { get; init; } = 50;

    public double LeftMargin { get; init; } = 10;

    public double TopMargin { get; init; } = 10;

    public double RightMargin { get; init; } = 10;

    public double BottomMargin { get; init; } = 10;

    public bool IsTopMost { get; init; } = true;

    /// <summary>是否让鼠标操作穿过透明窗口。</summary>
    public bool IsClickThrough { get; init; }
}
