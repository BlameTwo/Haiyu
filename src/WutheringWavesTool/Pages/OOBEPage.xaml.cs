namespace Haiyu.Pages;

public sealed partial class OOBEPage : Page, IDisposable
{
    private bool _disposed;

    public OOBEPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<OOBEViewModel>()!;
        this.ViewModel.NavigationService.RegisterView(this.frame);
    }

    public OOBEViewModel ViewModel { get; }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        ViewModel.NavigationService.UnRegisterView();
        Bindings.StopTracking();
    }
}
