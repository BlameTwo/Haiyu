using Microsoft.UI.Composition.SystemBackdrops;
using WinUIEx.Messaging;

namespace Haiyu;

public sealed partial class MainWindow : WinUIEx.WindowEx
{
    private WindowMessageMonitor mon;
    private const int WM_NCLBUTTONDBLCLK = 0x00A3; // Non-client left button double-click
    private const int WM_SYSCOMMAND = 0x0112; // System command message
    private const int SC_MAXIMIZE = 0xF030; // Maximize command
    private const int WM_SIZE = 0x0005; // Resize message
    private const int SIZE_MAXIMIZED = 2; // Maximized size
    private const int GWLP_WNDPROC = -4;
    public MainWindow()
    {
        this.InitializeComponent();
        this.AppWindow.SetIcon(AppDomain.CurrentDomain.BaseDirectory + "Assets/appLogo.ico");
        this.AppWindow.Title = "鸣潮启动器 1.2.11";
        this.IsResizable = false;
        NativeWindowHelper.ForceDisableMaximize(this);
    }

    
}
