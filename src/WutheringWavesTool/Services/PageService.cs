using WutheringWavesTool.Pages.GamePages;
using WutheringWavesTool.ViewModel.GameViewModels;

namespace WutheringWavesTool.Services;

public sealed partial class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages;

    public PageService()
    {
        _pages = new();
        this.RegisterView<SettingPage, SettingViewModel>();
        this.RegisterView<CommunityPage, CommunityViewModel>();
        #region GameContext
        this.RegisterView<MainGamePage, MainGameViewModel>();
        this.RegisterView<GlobalGamePage, GlobalGameViewModel>();
        this.RegisterView<BiliBiliGamePage, BiliBiliGameViewModel>();
        this.RegisterView<MainPGRGamePage, MainPGRViewModel>();
        this.RegisterView<BiliBiliPGRGamePage, BiliBiliPGRGameViewModel>();
        this.RegisterView<GlobalPGRGamePage, GlobalPGRViewModel>();
        this.RegisterView<StatisticsPage, StatisticsViewModel>();
        #endregion
        this.RegisterView<GamerRoilsPage, GameRoilsViewModel>();
        this.RegisterView<GamerDockPage, GamerDockViewModel>();
        this.RegisterView<CloudGamePage, CloudGameViewModel>();
        this.RegisterView<GamerChallengePage, GamerChallengeViewModel>();
        this.RegisterView<GamerSlashDetailPage, GamerSlashDetailViewModel>();
        this.RegisterView<GamerExploreIndexPage, GamerExploreIndexViewModel>();
        this.RegisterView<GamerTowerPage, GamerTowerViewModel>();
        this.RegisterView<GamerSkinPage, GamerSkinViewModel>();
        this.RegisterView<ResourceBriefPage, ResourceBriefViewModel>();

        this.RegisterView<RecordItemPage, RecordItemViewModel>();
        this.RegisterView<TestPage, TestViewModel>();
    }

    public Type GetPage(string key)
    {
        _pages.TryGetValue(key, out var page);
        if (page is null)
        {
            return null;
        }
        return page;
    }

    public void RegisterView<View, ViewModel>()
        where View : Page,IPage
        where ViewModel : ObservableObject
    {
        var key = typeof(ViewModel).FullName;
        if (_pages.ContainsKey(key))
        {
            throw new ArgumentException("已注册ViewModel");
        }
        if (_pages.ContainsValue(typeof(View)))
        {
            throw new ArgumentException("已注册View");
        }
        _pages.Add(key: typeof(ViewModel).FullName, typeof(View));
    }
}
