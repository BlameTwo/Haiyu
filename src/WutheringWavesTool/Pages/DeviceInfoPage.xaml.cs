namespace Haiyu.Pages;

public sealed partial class DeviceInfoPage : Page, IWindowPage
{
    public DeviceInfoPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<DeviceInfoViewModel>();
        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
    }


    public DeviceInfoViewModel? ViewModel { get; }

    public void Dispose()
    {
    }

    public void SetData(object value)
    {
    }

    public void SetWindow(Window window)
    {
        title.Window = window;
        title.Window.Closed += Window_Closed;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        this.ViewModel?.Dispose();
    }
}
