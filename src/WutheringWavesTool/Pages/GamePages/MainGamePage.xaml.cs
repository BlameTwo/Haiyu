using System.Numerics;
using CommunityToolkit.WinUI.Animations;
using WutheringWavesTool.ViewModel.GameViewModels;

namespace WutheringWavesTool.Pages.GamePages;

public sealed partial class MainGamePage : Page, IPage
{
    public MainGamePage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<MainGameViewModel>();
    }

    public MainGameViewModel ViewModel { get; }

    public Type PageType => typeof(MainGamePage);

    private void SelectorBar_SelectionChanged(
        SelectorBar sender,
        SelectorBarSelectionChangedEventArgs args
    )
    {
        this.ViewModel.SelectTab(sender.SelectedItem.Text);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.ViewModel.Dispose();
        this.Bindings.StopTracking();
    }
}
