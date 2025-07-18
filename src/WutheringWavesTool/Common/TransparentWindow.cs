using Windows.Graphics;
using WinUIEx;
using WutheringWavesTool.Models.Enums;

namespace WutheringWavesTool.Common;

public partial class TransparentWindow : WindowEx
{
    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;
    private const int WS_BORDER = 0x00800000;
    private const int WS_THICKFRAME = 0x00040000;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_SYSMENU = 0x00080000;

    // 鼠标穿透相关常量
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;
    private const int WS_EX_TOOLWINDOW = 0x80;
    private const int LWA_ALPHA = 0;

    private bool _isClickThrough = false;

    public TransparentWindow()
    {
        this.SystemBackdrop = new WinUIEx.TransparentTintBackdrop();

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

        // 获取当前窗口样式
        var style = Win32.GetWindowLong(hwnd, GWL_STYLE);

        // 移除边框相关的样式
        style &= ~(WS_BORDER | WS_THICKFRAME | WS_CAPTION | WS_SYSMENU);

        // 应用新样式
        Win32.SetWindowLong(hwnd, GWL_STYLE, style);

        Win32.SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        Win32.SetLayeredWindowAttributes(hwnd, 0, 100, LWA_ALPHA);
        // 创建OverlappedPresenter并设置为无边框
        this.AppWindow.IsShownInSwitchers = false;
        // 应用到窗口
        if (this.AppWindow.Presenter is OverlappedPresenter over)
        {
            over.IsAlwaysOnTop = true;
        }
    }

    public void TransparentMove(Point newPostion)
    {
        this.Move((int)newPostion.X, (int)newPostion.Y);
    }

    public void SetSize(Size size)
    {
        this.SetWindowSize(size.Width, size.Height);
    }

    public void SetClickThrough(bool enabled)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var exStyle = Win32.GetWindowLong(hwnd, GWL_EXSTYLE);

        if (enabled)
        {
            // 启用鼠标穿透
            exStyle |= WS_EX_TRANSPARENT;
        }
        else
        {
            // 禁用鼠标穿透
            exStyle &= ~WS_EX_TRANSPARENT;
        }

        SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
        _isClickThrough = enabled;
    }

    public void SetPostion(PostionType type, int EDGE_MARGIN = 20)
    {
        try
        {
            // 获取主显示器信息
            var displayArea = DisplayArea.Primary;
            var workArea = displayArea.WorkArea;

            // 获取当前窗口大小
            var windowSize = this.AppWindow.Size;

            int x,
                y;

            switch (type)
            {
                case PostionType.LeftTop:
                    x = workArea.X + EDGE_MARGIN;
                    y = workArea.Y + EDGE_MARGIN;
                    break;

                case PostionType.RightTop:
                    x = workArea.X + workArea.Width - windowSize.Width - EDGE_MARGIN;
                    y = workArea.Y + EDGE_MARGIN;
                    break;

                case PostionType.LeftBottom:
                    x = workArea.X + EDGE_MARGIN;
                    y = workArea.Y + workArea.Height - windowSize.Height - EDGE_MARGIN;
                    break;

                case PostionType.RightBottom:
                    x = workArea.X + workArea.Width - windowSize.Width - EDGE_MARGIN;
                    y = workArea.Y + workArea.Height - windowSize.Height - EDGE_MARGIN;
                    break;

                default:
                    // 默认左上角
                    x = workArea.X + EDGE_MARGIN;
                    y = workArea.Y + EDGE_MARGIN;
                    break;
            }

            // 移动窗口到指定位置
            this.AppWindow.Move(new PointInt32(x, y));
            SetSize(new Size(200, 200));
        }
        catch (Exception ex)
        {
            // 如果获取显示器信息失败，使用默认位置
            //var defaultPositions = GetDefaultPositions();
            //if (defaultPositions.ContainsKey(type))
            //{
            //    var pos = defaultPositions[type];
            //    this.AppWindow.Move(new PointInt32(pos.X, pos.Y));
            //}
        }
    }
}
