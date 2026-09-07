using Haiyu.Models.Enums;
using Haiyu.Models.Options;
using Windows.Graphics;
using Windows.Win32.Foundation;

namespace Haiyu.Common.WindowContext;

/// <summary>
/// 按显示器工作区九宫格定位的透明窗口上下文。
/// </summary>
public sealed class PostionTransparentWindowContext : WindowContext
{
    public PostionTransparentWindowContext(IServiceScope service, string key)
        : base(service, key) { }

    public void ApplyOption()
    {
        if (Option is not PostionTransparentWindowOption option)
        {
            throw new InvalidOperationException(
                $"{nameof(PostionTransparentWindowContext)} 需要 {nameof(PostionTransparentWindowOption)}。"
            );
        }

        Window window = GetWindow();
        var workArea = WindowExtension.GetWorkarea()
            ?? throw new InvalidOperationException("未能获取显示器工作区。");
        double dpi = WindowExtension.GetScaleAdjustment(window);

        int configuredWidth = (int)Math.Round(option.Width * dpi);
        int configuredHeight = (int)Math.Round(option.Height * dpi);
        int leftMargin = (int)Math.Round(option.LeftMargin * dpi);
        int topMargin = (int)Math.Round(option.TopMargin * dpi);
        int rightMargin = (int)Math.Round(option.RightMargin * dpi);
        int bottomMargin = (int)Math.Round(option.BottomMargin * dpi);

        int availableLeft = workArea.Left + leftMargin;
        int availableTop = workArea.Top + topMargin;
        int availableWidth = workArea.Right - rightMargin - availableLeft;
        int availableHeight = workArea.Bottom - bottomMargin - availableTop;

        if (availableWidth <= 0 || availableHeight <= 0)
        {
            throw new InvalidOperationException("透明窗口边距超过了显示器工作区。");
        }

        int width = Math.Min(configuredWidth, availableWidth);
        int height = Math.Min(configuredHeight, availableHeight);
        int centerLeft = availableLeft + (availableWidth - width) / 2;
        int centerTop = availableTop + (availableHeight - height) / 2;
        int right = availableLeft + availableWidth - width;
        int bottom = availableTop + availableHeight - height;

        int left;
        int top;

        switch (option.Postion)
        {
            case PostionType.LeftTop:
                left = availableLeft;
                top = availableTop;
                break;
            case PostionType.TopCenter:
                left = availableLeft;
                top = availableTop;
                width = availableWidth;
                break;
            case PostionType.RightTop:
                left = right;
                top = availableTop;
                break;
            case PostionType.LeftCenter:
                left = availableLeft;
                top = availableTop;
                height = availableHeight;
                break;
            case PostionType.Center:
                left = centerLeft;
                top = centerTop;
                break;
            case PostionType.RightCenter:
                left = right;
                top = availableTop;
                height = availableHeight;
                break;
            case PostionType.LeftBottom:
                left = availableLeft;
                top = bottom;
                break;
            case PostionType.BottomCenter:
                left = availableLeft;
                top = bottom;
                width = availableWidth;
                break;
            case PostionType.RightBottom:
                left = right;
                top = bottom;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(option.Postion));
        }

        nint rawHwnd = WindowNative.GetWindowHandle(window);
        WindowExtension.SetWindowTopMost(new HWND(rawHwnd), option.IsTopMost);
        window.SetWindowSize(width / dpi, height / dpi);
        window.AppWindow.Move(new PointInt32(left, top));
    }
}
