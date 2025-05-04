using System.Threading.Tasks;
using CommunityToolkit.WinUI.Animations;
using Waves.Core.Common;
using WutheringWavesTool.Pages.GamePages;
using WutheringWavesTool.Services.DialogServices;
using WutheringWavesTool.Services.Navigations.NavigationViewServices;
using WutheringWavesTool.ViewModel.GameViewModels;

namespace WutheringWavesTool.ViewModel;

public sealed partial class ShellViewModel : ViewModelBase
{
    public ShellViewModel(
        [FromKeyedServices(nameof(HomeNavigationService))] INavigationService homeNavigationService,
        [FromKeyedServices(nameof(HomeNavigationViewService))]
            INavigationViewService homeNavigationViewService,
        ITipShow tipShow,
        IAppContext<App> appContext,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IViewFactorys viewFactorys,
        IWavesClient wavesClient
    )
    {
        HomeNavigationService = homeNavigationService;
        HomeNavigationViewService = homeNavigationViewService;
        TipShow = tipShow;
        AppContext = appContext;
        DialogManager = dialogManager;
        ViewFactorys = viewFactorys;
        WavesClient = wavesClient;
        RegisterMessanger();
    }

    public INavigationService HomeNavigationService { get; }
    public INavigationViewService HomeNavigationViewService { get; }
    public ITipShow TipShow { get; }
    public IAppContext<App> AppContext { get; }
    public IDialogManager DialogManager { get; }
    public IViewFactorys ViewFactorys { get; }
    public IWavesClient WavesClient { get; }

    [ObservableProperty]
    public partial string ServerName { get; set; }

    [ObservableProperty]
    public partial object SelectItem { get; set; }

    [ObservableProperty]
    public partial Visibility LoginBthVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility GamerRoleListsVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility CommunitySelectItemVisiblity { get; set; } = Visibility.Collapsed;
    public Controls.ImageEx Image { get; set; }
    public Border BackControl { get; internal set; }

    [ObservableProperty]
    public partial ObservableCollection<GameRoilDataWrapper> Roles { get; set; }

    [ObservableProperty]
    public partial GameRoilDataWrapper SelectRoles { get; set; }

    private void RegisterMessanger()
    {
        this.Messenger.Register<LoginMessanger>(this, LoginMessangerMethod);
    }

    partial void OnSelectRolesChanged(GameRoilDataWrapper value)
    {
        if (value == null)
            return;
        this.WavesClient.CurrentRoil = value;
        WeakReferenceMessenger.Default.Send<SwitchRoleMessager>(new SwitchRoleMessager(value));
    }

    [RelayCommand]
    void OpenMain()
    {
        this.HomeNavigationService.NavigationTo<MainGameViewModel>(
            null,
            new DrillInNavigationTransitionInfo()
        );
    }

    [RelayCommand]
    void BackPage()
    {
        if (HomeNavigationService.CanGoBack)
            HomeNavigationService.GoBack();
    }

    [RelayCommand]
    void OpenCommunity()
    {
        this.HomeNavigationService.NavigationTo<CommunityViewModel>(
            "Community",
            new EntranceNavigationTransitionInfo()
        );

        ServerName = "库街区";
    }

    [RelayCommand]
    void OpenSetting()
    {
        this.HomeNavigationService.NavigationTo<SettingViewModel>(
            "Setting",
            new DrillInNavigationTransitionInfo()
        );
    }

    [RelayCommand]
    void OpenTest()
    {
        this.HomeNavigationService.NavigationTo<TestViewModel>(
            "Setting",
            new DrillInNavigationTransitionInfo()
        );
    }

    [RelayCommand]
    void OpenPlayerRecordWindow()
    {
        var win = ViewFactorys.ShowPlayerRecordWindow();
        (win.AppWindow.Presenter as OverlappedPresenter)!.IsMaximizable = false;
        (win.AppWindow.Presenter as OverlappedPresenter)!.IsMinimizable = false;
        win.SystemBackdrop = new MicaBackdrop();
        win.AppWindowApp.Show();
    }

    [RelayCommand]
    async Task Login()
    {
        await DialogManager.ShowLoginDialogAsync();
    }

    private async void LoginMessangerMethod(object recipient, LoginMessanger message)
    {
        this.LoginBthVisibility = Visibility.Collapsed;
        CommunitySelectItemVisiblity = Visibility.Visible;
        await RefreshRoleLists();
        await Task.Delay(800);
        this.AppContext.MainTitle.UpDate();
    }

    public async Task RefreshRoleLists()
    {
        var gamers = await WavesClient.GetWavesGamerAsync(this.CTS.Token);
        if (gamers == null || gamers.Code != 200)
            return;
        this.Roles = await FormatRoilAsync(gamers.Data);
        this.SelectRoles = Roles[0];
        this.GamerRoleListsVisibility = Visibility.Visible;
        this.AppContext.MainTitle.UpDate();
    }

    [RelayCommand]
    async Task Loaded()
    {
        var network = await NetworkCheck.PingAsync("https://baidu.com");
        if (network == null || network.Status != System.Net.NetworkInformation.IPStatus.Success)
        {
            MessageBox.Show("网络未连接");
            return;
        }
        var result = await WavesClient.IsLoginAsync(this.CTS.Token);
        if (!result)
        {
            this.LoginBthVisibility = Visibility.Visible;
            CommunitySelectItemVisiblity = Visibility.Collapsed;
        }
        else
        {
            this.LoginBthVisibility = Visibility.Collapsed;
            CommunitySelectItemVisiblity = Visibility.Visible;
            this.GamerRoleListsVisibility = Visibility.Visible;
            await this.RefreshRoleLists();
        }
        //await Task.Delay(800);
        this.AppContext.MainTitle.UpDate();
        OpenMain();
    }

    async Task<ObservableCollection<GameRoilDataWrapper>> FormatRoilAsync(
        List<GameRoilDataItem> roilDataItems
    )
    {
        ObservableCollection<GameRoilDataWrapper> values = new();
        foreach (var item in roilDataItems)
        {
            GameRoilDataWrapper value = new GameRoilDataWrapper(item);
            var level = await WavesClient.GetGamerBassDataAsync(item, this.CTS.Token);
            value.GameLevel = level!.Level;
            values.Add(value);
        }
        return values;
    }

    [RelayCommand]
    async Task UnLogin()
    {
        AppSettings.Token = "";
        AppSettings.TokenId = "";
        WeakReferenceMessenger.Default.Send<UnLoginMessager>();
        this.GamerRoleListsVisibility = Visibility.Collapsed;

        await Loaded();
    }

    [RelayCommand]
    void OpenSignWindow()
    {
        var win = ViewFactorys.ShowSignWindow(this.SelectRoles.Item);
        win.MaxHeight = 350;
        win.MaxWidth = 350;
        (win.AppWindow.Presenter as OverlappedPresenter)!.IsMaximizable = false;
        (win.AppWindow.Presenter as OverlappedPresenter)!.IsMinimizable = false;
        win.AppWindowApp.Show();
    }

    internal void SetSelectItem(Type sourcePageType)
    {
        var page = this.HomeNavigationViewService.GetSelectItem(sourcePageType);
        SelectItem = page;
    }
}
