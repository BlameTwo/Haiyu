using Haiyu.Common.Contracts;
using Haiyu.ViewModel.ToolkitsViewModel;

namespace Haiyu.Pages.Toolkits;

public sealed partial class MonitorSettingPage : Page, IWindowInitializable
{
    private bool _disposed;

    public MonitorSettingPage(MonitorSettingViewModel viewModel)
    {
        InitializeComponent();
        this.ViewModel = viewModel;
        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
    }

    public MonitorSettingViewModel? ViewModel { get; private set; }

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
            this.ViewModel = null;
        }
    }

    public void Initialize()
    {
    }
}
