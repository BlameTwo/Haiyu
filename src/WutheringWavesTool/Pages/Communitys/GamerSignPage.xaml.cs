using Haiyu.Common.Contracts;
using Haiyu.ViewModel.Communitys;

namespace Haiyu.Pages.Communitys;

public sealed partial class GamerSignPage : Page
{
    private bool _disposed;

    public GamerSignPage(GamerSignViewModel viewModel)
    {
        this.InitializeComponent();
        this.ViewModel = viewModel;

        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
    }

    public GamerSignViewModel? ViewModel { get; private set; }


    public void SetWindow(Window window)
    {
        //TODO Window
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
            this.ViewModel = null;
        }
    }
}
