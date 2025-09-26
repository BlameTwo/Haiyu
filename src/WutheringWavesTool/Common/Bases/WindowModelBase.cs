using Microsoft.UI.Xaml;

namespace Haiyu.Common.Bases;

public partial class WindowModelBase :Window
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
            Microsoft.UI.Windowing.OverlappedPresenter presenter = (
                AppWindowApp.Presenter as Microsoft.UI.Windowing.OverlappedPresenter
            )!;
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            presenter.IsResizable = false;
            IntPtr baseHwnd = value;
            WindowExtension.SetWindowLong(hWnd, WindowExtension.GWL_HWNDPARENT, baseHwnd);
            presenter.IsModal = true;
            this.Closed += (s, e) =>
            {
                this.Content = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            };
        }
    }
}
