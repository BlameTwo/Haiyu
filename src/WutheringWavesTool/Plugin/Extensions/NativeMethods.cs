using System;
using System.Runtime.InteropServices;

namespace Haiyu.Plugin.Extensions;

public class NativeMethods
{
    [DllImport("gdi32.dll", EntryPoint = "CreateDC", CharSet = CharSet.Auto)]
    public static extern IntPtr CreateDC(
        string lpszDriver,
        string lpszDevice,
        string lpszOutput,
        IntPtr lpInitData
    );

    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
    public static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    public static extern bool DeleteObject(IntPtr hObject);

    public const int SRCCOPY = 0x00CC0020;

    [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
    public static extern bool BitBlt(
        IntPtr hdcDest,
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        int dwRop
    );

    [DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps")]
    public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    public const int HORZRES = 8;
    public const int VERTRES = 10;

    public const uint DIB_RGB_COLORS = 0;
    public const uint DIB_PAL_COLORS = 1;

    [DllImport("gdi32.dll", EntryPoint = "GetDIBits", SetLastError = true)]
    public static extern int GetDIBits(
        IntPtr hdc,
        IntPtr hbm,
        uint uStartScan,
        uint cScanLines,
        [Out] byte[] lpvBits,
        IntPtr lpbmi,
        uint uUsage
    );

    [DllImport("user32.dll", EntryPoint = "MessageBox", CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
    }
}
