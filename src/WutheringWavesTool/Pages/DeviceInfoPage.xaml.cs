using Haiyu.Common.Contracts;

namespace Haiyu.Pages;

public sealed partial class DeviceInfoPage : Page
{
    private bool _disposed;

    public DeviceInfoPage(DeviceInfoViewModel viewModel)
    {
        InitializeComponent();
        this.ViewModel = viewModel;
        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
    }

    public DeviceInfoViewModel? ViewModel { get; private set; }

    public void SetData(object value)
    {
    }


    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        try
        {
            this.Bindings.StopTracking();
            this.ViewModel?.Dispose();
        }
        finally
        {
            title.Window = null;
            this.ViewModel = null;
        }
    }
}
