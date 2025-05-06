using WutheringWavesTool.ViewModel.GameViewModels;

namespace WutheringWavesTool.Pages.GamePages;

public sealed partial class GlobalGamePage : Page, IPage
{
    public GlobalGamePage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<GlobalGameViewModel>();
    }

    public Type PageType => typeof(GlobalGamePage);

    public GlobalGameViewModel ViewModel { get; }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.ViewModel.Dispose();
        this.Bindings.StopTracking();
    }

    private void SelectorBar_SelectionChanged(
        SelectorBar sender,
        SelectorBarSelectionChangedEventArgs args
    )
    {
        this.ViewModel.SelectTab(sender.SelectedItem.Text);
    }
}
