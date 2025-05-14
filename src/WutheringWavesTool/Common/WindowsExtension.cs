namespace WutheringWavesTool.Common;

public static partial class WindowsExtension
{
    [SuppressMessage("CodeQuality", "IDE0079:请删除不必要的忽略")]
    private const int WS_EX_LAYERED = 0x80000;

    [SuppressMessage("CodeQuality", "IDE0079:请删除不必要的忽略")]
    private const int GWL_EXSTYLE = -20;

    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial long GetWindowLongA(nint hWnd, int nIndex);

    [LibraryImport("user32.dll")]
    public static partial int SetWindowLongA(nint hWnd, int nIndex, long dwNewLong);

    public static void SetLayerWindow(this Window window)
    {
        var hWnd = (nint)window.AppWindow.Id.Value;
        var exStyle = GetWindowLongA(hWnd, GWL_EXSTYLE);
        if ((exStyle & WS_EX_LAYERED) is 0)
            _ = SetWindowLongA(hWnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);
        var style = GetWindowLongA(hWnd, GWL_STYLE);
        _ = SetWindowLongA(hWnd, GWL_STYLE, style & ~WS_OVERLAPPEDWINDOW);
    }

    public const int GWL_STYLE = -16;
    public const uint WS_CAPTION = 0x00C00000;
    public const uint WS_MAXIMIZEBOX = 0x00010000;
    public const uint WS_MINIMIZEBOX = 0x00020000;
    public const uint WS_OVERLAPPED = 0x00000000;

    public const uint WS_OVERLAPPEDWINDOW = (
        WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX
    );

    public const uint WS_SYSMENU = 0x00080000;
    public const uint WS_THICKFRAME = 0x00040000;
}
