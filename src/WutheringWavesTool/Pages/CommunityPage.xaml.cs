namespace Haiyu.Pages;

public sealed partial class CommunityPage : Page, IPage, IDisposable
{
    private bool disposedValue;

    public CommunityPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<CommunityViewModel>();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        if (this.frame.Content is IDisposable disposable)
        {
            disposable.Dispose();
        }
        this.Dispose();
        GC.Collect();
        base.OnNavigatedFrom(e);
    }

    public Type PageType => typeof(CommunityPage);

    public CommunityViewModel ViewModel { get; private set; }

    private void dataSelect_SelectionChanged(
        SelectorBar sender,
        SelectorBarSelectionChangedEventArgs args
    )
    {
        if (sender.SelectedItem.Tag == null)
            return;
        #region 旧代码

        //switch (sender.SelectedItem.Tag.ToString())
        //{
        //    case "DataGamer":
        //        ViewModel.NavigationService.NavigationTo<GameRoilsViewModel>(
        //            this.ViewModel.SelectRoil.Item,
        //            new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
        //        );
        //        break;
        //    case "DataDock":
        //        ViewModel.NavigationService.NavigationTo<GamerDockViewModel>(
        //            this.ViewModel.SelectRoil.Item,
        //            new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
        //        );
        //        break;
        //    case "DataChallenge":
        //        ViewModel.NavigationService.NavigationTo<GamerChallengeViewModel>(
        //            this.ViewModel.SelectRoil.Item,
        //            new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
        //        );
        //        break;
        //    case "DataAbyss":
        //        ViewModel.NavigationService.NavigationTo<GamerTowerViewModel>(
        //            this.ViewModel.SelectRoil.Item,
        //            new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
        //        );
        //        break;
        //    case "DataWorld":
        //        ViewModel.NavigationService.NavigationTo<GamerExploreIndexViewModel>(
        //            this.ViewModel.SelectRoil.Item,
        //            new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
        //        );
        //        break;
        //    case "Skin":
        //        ViewModel.NavigationService.NavigationTo<GamerSkinViewModel>(
        //            this.ViewModel.SelectRoil.Item,
        //            new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
        //        );
        //        break;
        //}
        #endregion
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.ViewModel.NavigationService.UnRegisterView();
                this.ViewModel.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
