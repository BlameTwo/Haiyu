using Microsoft.UI.Composition.SystemBackdrops;
using WinUIEx.Messaging;

namespace WutheringWavesTool
{
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadCursorFromFile(string lpFileName);

        [DllImport("user32.dll")]
        public static extern int SetClassLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        public static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);
        private const int GCL_HCURSOR = -12; // 类光标
        private const int GWL_HCURSOR = -8;  // 窗口实例光标
        private WindowMessageMonitor monitor;

        public MainWindow()
        {
            this.InitializeComponent();
            this.AppWindow.SetIcon(AppDomain.CurrentDomain.BaseDirectory + "Assets/appLogo.ico");
            this.AppWindow.Title = "鸣潮启动器-Dev";
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            IntPtr hCursor = LoadCursorFromFile(AppDomain.CurrentDomain.BaseDirectory+"Assets/Cursor/ShouAnRenPostion.ani");
            if (hCursor == IntPtr.Zero)
            {
                throw new Exception("无法加载自定义光标文件。");
            }
            SetClassLongPtr(hWnd, GCL_HCURSOR, hCursor);
            UpdateWindow(hWnd);
        }



    }
}
