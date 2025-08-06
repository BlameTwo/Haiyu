namespace WutheringWavesTool.Pages;

public sealed partial class DeviceInfoPage : Page, IWindowPage
{
    public DeviceInfoPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<DeviceInfoViewModel>();
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
    }
}
