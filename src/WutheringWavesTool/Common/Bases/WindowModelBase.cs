namespace Haiyu.Common.Bases;

public partial class WindowModelBase : Window
{
    public AppWindow AppWindowApp;

    OverlappedPresenter? Overlapped => this.AppWindow.Presenter as OverlappedPresenter;

    public WindowManager Manager => WindowManager.Get(this);

    public WindowModelBase(nint value)
    {
        this.SystemBackdrop = new DesktopAcrylicBackdrop();
        if (Overlapped != null)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            Microsoft.UI.WindowId windowId1 = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindowApp = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId1);
            IntPtr baseHwnd = value;
            WindowExtension.SetWindowLong(hWnd, WindowExtension.GWL_HWNDPARENT, baseHwnd);
            Microsoft.UI.Windowing.OverlappedPresenter presenter = OverlappedPresenter.CreateForDialog();
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            presenter.IsResizable = false;
            presenter.IsModal = true;
            this.AppWindow.SetPresenter(presenter);
            this.Closed += (s, e) =>
            {
                var windowId = Win32Interop.GetWindowIdFromWindow(baseHwnd);
                var parentAppWindow = AppWindow.GetFromWindowId(windowId);
                parentAppWindow.Show();
                WindowExtension.SwitchToThisWindow(baseHwnd,true);
            };
        }
    }
}
