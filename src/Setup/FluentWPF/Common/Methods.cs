using System.Runtime.InteropServices;

namespace FluentWPF.Common;

public static class Methods
{
    public const int WS_MINIMIZEBOX = 131072;

    public const int WS_MAXIMIZEBOX = 65536;

    public const int WS_EXIT = 0x80000;

    public const int WS_SYSMENU = 524288;


    public const int GWL_STYLE = -16;

    public const int SW_SHOWNORMAL = 1;

    public const int SC_CLOSE = 61536;

    public const int MF_ENABLED = 0;

    public const int MF_GRAYED = 1;

    public const int MF_DISABLED = 2;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32")]
    public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, int NewLong);

    [DllImport("user32.dll")]
    public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

    [DllImport("user32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SetWindowText(IntPtr hwnd, string lpString);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool ShowWindow(IntPtr hWnd, short State);

    [DllImport("user32.dll")]
    public static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    [DllImport("dwmapi.dll")]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

    [Flags]
    public enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        DWMWA_SYSTEMBACKDROP_TYPE = 38
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int cxLeftWidth;      // width of left border that retains its size
        public int cxRightWidth;     // width of right border that retains its size
        public int cyTopHeight;      // height of top border that retains its size
        public int cyBottomHeight;   // height of bottom border that retains its size
    };


    public struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;

        public IntPtr Data;

        public int SizeOfData;
    }

    public enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll")]
    public static extern int GetMenuItemCount(IntPtr hMenu);

    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    public static extern int GetSystemMetricsForDpi(int nIndex, uint dpi);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    public const int SM_CXSIZE = 30;        // SystemMetrics index for the width of a window's sizing border
    public const int SM_CXMINBUTTON = 15;   // SystemMetrics index for the width of a window's minimize button
    public const int SM_CXMAXBUTTON = 17;   // SystemMetrics index for the width of a window's maximize button
    public const int SM_CYSIZE = 31;  // 标题栏按钮的高度
    public const int SM_CXSIZEFRAME = 32;  // Width of the window frame
    public const int LOGPIXELSX = 88;  // Logical pixels/inch in X

    public const int SM_CXFRAME = 32; // 窗口边框的宽度
    public const int SM_CXPADDEDBORDER = 92; // 窗口边框的额外宽度


}
