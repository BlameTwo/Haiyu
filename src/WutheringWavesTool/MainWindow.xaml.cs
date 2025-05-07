using Microsoft.UI.Composition.SystemBackdrops;

namespace WutheringWavesTool
{
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.AppWindow.SetIcon(AppDomain.CurrentDomain.BaseDirectory + "Assets/appLogo.ico");
            this.AppWindow.Title = "鸣潮启动器-Dev";
        }
    }
}
