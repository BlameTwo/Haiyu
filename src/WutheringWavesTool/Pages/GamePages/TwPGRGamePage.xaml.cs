using Haiyu.ViewModel.GameViewModels;
namespace Haiyu.Pages.GamePages;

public sealed partial class TwPGRGamePage : Page, IPage
{
    public TwPGRGamePage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<TwPGRGameViewModel>();
    }

    private void SelectorBar_SelectionChanged(
        SelectorBar sender,
        SelectorBarSelectionChangedEventArgs args
    )
    {
        this.ViewModel.SelectTab(sender.SelectedItem.Text);
    }

    public TwPGRGameViewModel ViewModel { get; }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.ViewModel.Dispose();
        this.Bindings.StopTracking();
    }

    public Type PageType => typeof(MainPGRGamePage);
}
