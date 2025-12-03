namespace Haiyu;

public sealed partial class MainWindow : WinUIEx.WindowEx
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.AppWindow.SetIcon(AppDomain.CurrentDomain.BaseDirectory + "Assets/appLogo.ico");
        this.IsResizable = false;
        NativeWindowHelper.ForceDisableMaximize(this);
       
    }


}
