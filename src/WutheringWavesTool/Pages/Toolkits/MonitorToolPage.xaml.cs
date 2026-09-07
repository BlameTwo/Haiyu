using Haiyu.Common.Contracts;
using Haiyu.ViewModel.ToolkitsViewModel;

namespace Haiyu.Pages.Toolkits;

public sealed partial class MonitorToolPage : Page,IWindowInitializable
{
    private bool _disposed;

    public MonitorToolPage(MonitorToolViewModel viewModel, WindowSession session)
    {
        InitializeComponent();
        this.ViewModel = viewModel;
        Session = session;
        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
    }

    public MonitorToolViewModel? ViewModel { get; private set; }

    public WindowSession Session { get; }

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
        if (ViewModel is not null)
        {
            ViewModel.Window = Session.Context.GetWindow();
        }
    }
}
