using System.Linq;
using Astronomical;
using Haiyu.Models.Wrapper;
using Haiyu.Services.DialogServices;
using Waves.Core.Common;
using Waves.Core.Models.Enums;
using Windows.Devices.Geolocation;
using Windows.Graphics.DirectX.Direct3D11;

namespace Haiyu.ViewModel;

public sealed partial class ShellViewModel : ViewModelBase
{
    private bool computerShow;

    public ShellViewModel(
        [FromKeyedServices(nameof(HomeNavigationService))] INavigationService homeNavigationService,
        [FromKeyedServices(nameof(HomeNavigationViewService))]
            INavigationViewService homeNavigationViewService,
        ITipShow tipShow,
        IAppContext<App> appContext,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IViewFactorys viewFactorys,
        IKuroClient wavesClient,
        ILauncherTaskService launcherTaskService
    )
    {
        HomeNavigationService = homeNavigationService;
        HomeNavigationViewService = homeNavigationViewService;
        TipShow = tipShow;
        AppContext = appContext;
        DialogManager = dialogManager;
        ViewFactorys = viewFactorys;
        WavesClient = wavesClient;
        LauncherTaskService = launcherTaskService;
        RegisterMessanger();
        SystemMenu = new NotifyIconMenu()
        {
            Items = new List<NotifyIconMenuItem>()
            {
                new() { Header = "显示主界面", Command = this.ShowWindowCommand },
                new() { Header = "退出启动器", Command = this.ExitWindowCommand },
            },
        };
    }

    [ObservableProperty]
    public partial NotifyIconMenu SystemMenu { get; set; }

    public INavigationService HomeNavigationService { get; }
    public INavigationViewService HomeNavigationViewService { get; }
    public ITipShow TipShow { get; }
    public IAppContext<App> AppContext { get; }
    public IDialogManager DialogManager { get; }
    public IViewFactorys ViewFactorys { get; }
    public IKuroClient WavesClient { get; }
    public ILauncherTaskService LauncherTaskService { get; }

    private IDirect3DDevice _device;

    [ObservableProperty]
    public partial string ServerName { get; set; }

    [ObservableProperty]
    public partial object SelectItem { get; set; }

    [ObservableProperty]
    public partial Visibility LoginBthVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility GamerRoleListsVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility WavesCommunitySelectItemVisiblity { get; set; } =
        Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility PunishCommunitySelectItemVisiblity { get; set; } =
        Visibility.Collapsed;

    public Controls.ImageEx Image { get; set; }
    public Border BackControl { get; internal set; }

    [ObservableProperty]
    public partial ObservableCollection<GameRoilDataWrapper> Roles { get; set; }

    [ObservableProperty]
    public partial CollectionViewSource RoleViewSource { get; set; }

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
        if (value.Type == Waves.Core.Models.Enums.GameType.Waves)
        {
            this.WavesCommunitySelectItemVisiblity = Visibility.Visible;
            this.PunishCommunitySelectItemVisiblity = Visibility.Collapsed;
        }
        else if (value.Type == Waves.Core.Models.Enums.GameType.Punish)
        {
            this.WavesCommunitySelectItemVisiblity = Visibility.Collapsed;
            this.PunishCommunitySelectItemVisiblity = Visibility.Visible;
        }
        WeakReferenceMessenger.Default.Send<SwitchRoleMessager>(new SwitchRoleMessager(value));
    }

    [RelayCommand]
    void OpenMain()
    {
        this.HomeNavigationService.NavigationTo<HomeViewModel>(
            null,
            new DrillInNavigationTransitionInfo()
        );
    }

    [RelayCommand]
    void OpenColorGame()
    {
        var result = this.ViewFactorys.ShowColorGame();
        result.Manager.MaxHeight = 600;
        result.Manager.MaxWidth = 1000;
        result.Manager.Height = 600;
        result.Manager.Width = 1000;
        result.Activate();
    }

    [RelayCommand]
    void OpenStartColorGame()
    {
        var result = this.ViewFactorys.ShowStartColorGame();
        result.Manager.MaxHeight = 600;
        result.Manager.MaxWidth = 1000;
        result.Manager.Height = 600;
        result.Manager.Width = 1000;
        result.Activate();
    }

    [RelayCommand]
    void BackPage()
    {
        if (HomeNavigationService.CanGoBack)
            HomeNavigationService.GoBack();
    }

    [RelayCommand]
    void Min()
    {
        this.AppContext.Minimise();
    }

    [RelayCommand]
    void CloseWindow()
    {
        this.AppContext.CloseAsync();
    }

    [RelayCommand]
    void ShowWindow()
    {
        this.AppContext.App.MainWindow.Show();
    }

    [RelayCommand]
    void ExitWindow()
    {
        Environment.Exit(0);
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
    async Task OpenScreenCapture()
    {
        var result = await DialogManager.GetQRLoginResultAsync();
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
        win.Activate();
    }

    [RelayCommand]
    async Task Login()
    {
        await DialogManager.ShowLoginDialogAsync();
    }

    [RelayCommand]
    async Task LoginWebGame()
    {
        await DialogManager.ShowWebGameDialogAsync();
    }

    private async void LoginMessangerMethod(object recipient, LoginMessanger message)
    {
        this.LoginBthVisibility = Visibility.Collapsed;
        WavesCommunitySelectItemVisiblity = Visibility.Visible;
        await RefreshRoleLists();
        await Task.Delay(800);
        this.AppContext.MainTitle.UpDate();
    }

    [RelayCommand]
    public async Task RefreshRoleLists()
    {
        var gamers = await WavesClient.GetGamerAsync(GameType.Waves,this.CTS.Token);
        var punishs = await WavesClient.GetGamerAsync(GameType.Punish,this.CTS.Token);
        if (gamers == null || gamers.Code != 200 || punishs == null || punishs.Code != 200)
            return;
        this.Roles = gamers
            .Data.FormatRoil(Waves.Core.Models.Enums.GameType.Waves)
            .Concat(punishs.Data.FormatRoil(Waves.Core.Models.Enums.GameType.Punish))
            .ToObservableCollection();
        if (Roles != null)
        {
            if (Roles.Count > 0)
                this.SelectRoles = Roles[0];
        }
        this.GamerRoleListsVisibility = Visibility.Visible;
        this.AppContext.MainTitle.UpDate();
    }

    [RelayCommand]
    async Task Loaded()
    {
        var network = await NetworkCheck.PingAsync(GameAPIConfig.BaseAddress[0]);
        if (network == null || network.Status != System.Net.NetworkInformation.IPStatus.Success)
        {
            Logger.WriteError($"检查库洛CDN服务器失败！，地址为:{GameAPIConfig.BaseAddress[0]}");
            Environment.Exit(0);
        }
        var result = await WavesClient.IsLoginAsync(this.CTS.Token);
        if (!result)
        {
            this.LoginBthVisibility = Visibility.Visible;
            WavesCommunitySelectItemVisiblity = Visibility.Collapsed;
            PunishCommunitySelectItemVisiblity = Visibility.Collapsed;
        }
        else
        {
            this.LoginBthVisibility = Visibility.Collapsed;
            WavesCommunitySelectItemVisiblity = Visibility.Visible;
            this.GamerRoleListsVisibility = Visibility.Visible;
            await this.RefreshRoleLists();
        }
        this.AppContext.MainTitle.UpDate();
        this.ShowPGRBilibiliGame = AppSettings.ShowPGRBilibiliGame;
        this.ShowPGRGlobalGame = AppSettings.ShowPGRGlobalGame;
        this.ShowPGRMainGame = AppSettings.ShowPGRMainGame;
        this.ShowWavesMainGame = AppSettings.ShowWavesMainGame;
        this.ShowWavesGlobalGame = AppSettings.ShowWavesGlobalGame;
        this.ShowWavesBilibiliGame = AppSettings.ShowWavesBilibiliGame;
        this.ShowTwPGRGame = AppSettings.ShowTwPGRGame;
        OpenMain();
        await LauncherTaskService.RunAsync(this.CTS.Token);
    }

    [RelayCommand]
    public void ShowDeviceInfo()
    {
        var window = ViewFactorys.ShowAdminDevice();
        window.Activate();
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
        win.Manager.MaxHeight = 350;
        win.Manager.MaxWidth = 350;
        (win.AppWindow.Presenter as OverlappedPresenter)!.IsMaximizable = false;
        (win.AppWindow.Presenter as OverlappedPresenter)!.IsMinimizable = false;
        win.AppWindowApp.Show();
    }

    [RelayCommand]
    void OpenCounter(RoutedEventArgs args) { }

    private void ComputerWin_Closed(object sender, WindowEventArgs args) { }

    internal void SetSelectItem(Type sourcePageType)
    {
        var page = this.HomeNavigationViewService.GetSelectItem(sourcePageType);
        SelectItem = page;
    }

    #region 鸣潮
    [ObservableProperty]
    public partial bool? ShowWavesMainGame { get; set; }

    partial void OnShowWavesMainGameChanged(bool? value)
    {
        AppSettings.ShowWavesMainGame = value;
    }

    [ObservableProperty]
    public partial bool? ShowWavesBilibiliGame { get; set; }

    partial void OnShowWavesBilibiliGameChanged(bool? value)
    {
        AppSettings.ShowWavesBilibiliGame = value;
    }

    [ObservableProperty]
    public partial bool? ShowWavesGlobalGame { get; set; }

    partial void OnShowWavesGlobalGameChanged(bool? value)
    {
        AppSettings.ShowWavesGlobalGame = value;
    }
    #endregion

    #region 战双帕弥什
    [ObservableProperty]
    public partial bool? ShowPGRMainGame { get; set; }

    partial void OnShowPGRMainGameChanged(bool? value)
    {
        AppSettings.ShowPGRMainGame = value;
    }

    [ObservableProperty]
    public partial bool? ShowPGRBilibiliGame { get; set; }

    partial void OnShowPGRBilibiliGameChanged(bool? value)
    {
        AppSettings.ShowPGRBilibiliGame = value;
    }

    [ObservableProperty]
    public partial bool? ShowPGRGlobalGame { get; set; }

    partial void OnShowPGRGlobalGameChanged(bool? value)
    {
        AppSettings.ShowPGRGlobalGame = value;
    }

    [ObservableProperty]
    public partial bool? ShowTwPGRGame { get; set; }

    partial void OnShowTwPGRGameChanged(bool? value)
    {
        AppSettings.ShowTwPGRGame = value;
    }
    #endregion
}
