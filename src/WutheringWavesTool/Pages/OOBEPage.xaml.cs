namespace Haiyu.Pages;

public sealed partial class OOBEPage : Page
{
    public OOBEPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.Host.Services.GetRequiredService<OOBEViewModel>();
        this.ViewModel.NavigationService.RegisterView(this.frame);
    }

    public OOBEViewModel ViewModel { get; }
}
