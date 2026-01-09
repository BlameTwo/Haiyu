using Haiyu.Services.DialogServices;

namespace Haiyu.ViewModel;

public partial class CommunityViewModel : ViewModelBase, IDisposable
{
    public CommunityViewModel(
        IKuroClient wavesClient,
        IViewFactorys viewFactorys,
        [FromKeyedServices(nameof(CommunityNavigationService))]
            INavigationService navigationService,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager
    )
    {
        WavesClient = wavesClient;
        ViewFactorys = viewFactorys;
        NavigationService = navigationService;
        DialogManager = dialogManager;
        RegisterMessanger();
    }

    public IKuroClient WavesClient { get; }
    public IAppContext<App> AppContext { get; }
    public IViewFactorys ViewFactorys { get; }
    public INavigationService NavigationService { get; set; }
    public IDialogManager DialogManager { get; }

    [ObservableProperty]
    public partial bool IsLogin { get; set; }

    [ObservableProperty]
    public partial List<CommunitySwitchPageWrapper> Pages { get; set; } =
        CommunitySwitchPageWrapper.GetDefault();

    [ObservableProperty]
    public partial CommunitySwitchPageWrapper SelectPageItem { get; set; }

    [ObservableProperty]
    public partial bool DataLoad { get; set; } = false;
    public Window Window { get; internal set; }

    private void RegisterMessanger()
    {
        this.Messenger.Register<LoginMessanger>(this, LoginMessangerMethod);
        this.Messenger.Register<UnLoginMessager>(this, UnLoginMethod);
        this.Messenger.Register<ShowRoleData>(this, ShowRoleMethod);
    }

    private async void UnLoginMethod(object recipient, UnLoginMessager message)
    {
        await LoadedAsync();
    }

    partial void OnSelectPageItemChanged(CommunitySwitchPageWrapper value)
    {
        switch (value.Tag.ToString())
        {
            case "DataGamer":
                NavigationService.NavigationTo<GameRoilsViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
            case "DataDock":
                NavigationService.NavigationTo<GamerDockViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
            case "DataChallenge":
                NavigationService.NavigationTo<GamerChallengeViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
            case "DataAbyss":
                NavigationService.NavigationTo<GamerTowerViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
            case "DataWorld":
                NavigationService.NavigationTo<GamerExploreIndexViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
            case "Skin":
                NavigationService.NavigationTo<GamerSkinViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
            case "Boss2":
                NavigationService.NavigationTo<GamerSlashDetailViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
            case "Resource":
                NavigationService.NavigationTo<ResourceBriefViewModel>(
                    null,
                    new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo()
                );
                break;
        }
    }

    private void ShowRoleMethod(object recipient, ShowRoleData message)
    {
        ViewFactorys.ShowRolesDataWindow(message).Activate();
    }

    private async void LoginMessangerMethod(object recipient, LoginMessanger message)
    {
        await LoadedAsync();
    }

    [RelayCommand]
    async Task LoadedAsync(Frame frame = null)
    {
        if (frame != null)
            this.NavigationService.RegisterView(frame);
        this.IsLogin = (await WavesClient.IsLoginAsync());
        if (!IsLogin)
            return;
        var gamers = await WavesClient.GetGamerAsync(Waves.Core.Models.Enums.GameType.Waves,this.CTS.Token);
        if (gamers == null || gamers.Code != 200)
            return;
        this.SelectPageItem = Pages[0];
        this.DataLoad = true;
    }

    public override void Dispose()
    {
        this.Messenger.UnregisterAll(this);
        this.NavigationService.UnRegisterView();
        this.NavigationService = null;
        this.CTS.Cancel();
        GC.SuppressFinalize(this);
    }
}
