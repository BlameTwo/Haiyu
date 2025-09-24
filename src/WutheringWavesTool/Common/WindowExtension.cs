namespace Haiyu.Common;

public static class WindowExtension
{
    public enum CreateType
    {
        Subtitle,
    }

    public const uint WS_EX_LAYERED = 0x80000;
    public const uint WS_EX_TRANSPARENT = 0x20;
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;
    public const int LWA_ALPHA = 0;

    public const int GWL_HWNDPARENT = (-8);

    public static Window CreateTransparentWindow(Window win, CreateType type)
    {
        var window = new Window();
        var dpi = GetScaleAdjustment(win);
        var workArea = GetWorkarea();
        if (type == CreateType.Subtitle)
        {
            double height = 100;
            int leftMargin = 200;
            int rightMargin = 200;
            double width = workArea.Value.Right - workArea.Value.Left - leftMargin - rightMargin;
            int left = workArea.Value.Left + leftMargin;
            int top = workArea.Value.Bottom - (int)height;
            window.SetWindowSize(width / dpi, height / dpi);
            window.AppWindow.Move(new Windows.Graphics.PointInt32() { X = left, Y = top });
        }
        window.Activate();
        return window;
    }

    public static double GetScaleAdjustment(Window window)
    {
        nint hWnd = WindowNative.GetWindowHandle(window);
        Microsoft.UI.WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
        nint hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        int result = GetDpiForMonitor(
            hMonitor,
            Monitor_DPI_Type.MDT_Default,
            out uint dpiX,
            out uint _
        );
        if (result != 0)
        {
            throw new Exception("Could not get DPI for monitor.");
        }

        uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    public static void Penetrate(Window window)
    {
        nint hWnd = WindowNative.GetWindowHandle(window);
        GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
        SetLayeredWindowAttributes(hWnd, 0, 100, LWA_ALPHA);
    }

    public static void UnPenetrate(Window window)
    {
        nint hWnd = WindowNative.GetWindowHandle(window);
        uint currentExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        uint newExStyle = currentExStyle & ~WS_EX_TRANSPARENT;
        SetWindowLong(hWnd, GWL_EXSTYLE, newExStyle);
        SetLayeredWindowAttributes(hWnd, 0, 100, LWA_ALPHA);
    }

    [DllImport("Shcore.dll", SetLastError = true)]
    public static extern int GetDpiForMonitor(
        nint hmonitor,
        Monitor_DPI_Type dpiType,
        out uint dpiX,
        out uint dpiY
    );

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(nint hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32", EntryPoint = "GetWindowLong")]
    public static extern uint GetWindowLong(nint hwnd, int nIndex);

    [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
    public static extern int SetLayeredWindowAttributes(
        nint hwnd,
        int crKey,
        int bAlpha,
        int dwFlags
    );

    public enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI,
    }

    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    public const int SM_CXSCREEN = 0;
    public const int SM_CYSCREEN = 1;

    public static POINT GetCursorPosition()
    {
        POINT lpPoint;
        GetCursorPos(out lpPoint);
        return lpPoint;
    }

    public static RECT? GetWorkarea()
    {
        RECT workArea = new RECT();
        if (SystemParametersInfo(SPI_GETWORKAREA, 0, ref workArea, 0))
        {
            return workArea;
        }
        return null;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int ShellExecute(
        nint hwnd,
        string lpOperation,
        string lpFile,
        string lpParameters,
        string lpDirectory,
        ShowCommands nShowCmd
    );

    public enum ShowCommands : int
    {
        SW_HIDE = 0,
        SW_NORMAL = 1, // 默认值，正常方式启动
        SW_MAXIMIZE = 3, // 最大化窗口启动
        SW_MINIMIZE = 6, // 最小化窗口启动
        SW_SHOW = 5, // 显示窗口启动
        SW_SHOWDEFAULT = 10, // 使用默认设置启动
    }

    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

    public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (IntPtr.Size == 4)
        {
            return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
        }
        return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
    }

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
    public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    public static long GetWindowLongAtt(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 4)
        {
            return GetWindowLong32(hWnd, nIndex);
        }
        return GetWindowLongPtr64(hWnd, nIndex);
    }

    [DllImport("User32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
    public static extern long GetWindowLong32(IntPtr hWnd, int nIndex);

    [DllImport("User32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
    public static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);
}

public static partial class LayerWindowHelper
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("CodeQuality", "IDE0079:请删除不必要的忽略")]
    private const int WS_EX_LAYERED = 0x80000;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("CodeQuality", "IDE0079:请删除不必要的忽略")]
    private const int GWL_EXSTYLE = -20;

    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial int GetWindowLongA(nint hWnd, int nIndex);

    [LibraryImport("user32.dll")]
    public static partial int SetWindowLongA(nint hWnd, int nIndex, int dwNewLong);

    public static void SetLayerWindow(Window window)
    {
        var hWnd = (nint)window.AppWindow.Id.Value;
        var exStyle = GetWindowLongA(hWnd, GWL_EXSTYLE);
        if ((exStyle & WS_EX_LAYERED) is 0)
            _ = SetWindowLongA(hWnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);
    }
}
