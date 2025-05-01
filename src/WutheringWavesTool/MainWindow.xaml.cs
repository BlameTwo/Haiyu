using Microsoft.UI.Composition.SystemBackdrops;

namespace WutheringWavesTool
{
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        DesktopAcrylicController controller;

        public MainWindow()
        {
            this.InitializeComponent();
            (this.AppWindow.Presenter as OverlappedPresenter).SetBorderAndTitleBar(true, false);
        }
    }
}
