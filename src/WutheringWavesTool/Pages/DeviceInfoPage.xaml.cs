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

}
