using WutheringWavesTool.ViewModel.GameViewModels;

namespace WutheringWavesTool.Pages.GamePages;

public sealed partial class BiliBiliGamePage : Page, IPage
{
    public BiliBiliGamePage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<BiliBiliGameViewModel>();
    }

    public Type PageType => typeof(BiliBiliGamePage);

    public BiliBiliGameViewModel ViewModel { get; }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.ViewModel.Dispose();
        this.Bindings.StopTracking();
    }
}
