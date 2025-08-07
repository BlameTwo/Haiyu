using CommunityToolkit.WinUI.Animations;
using H.NotifyIcon;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.DirectX;
using System.Drawing;
using System.Threading.Tasks;
using Waves.Core.Common;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;
using WutheringWavesTool.Common.QR;
using WutheringWavesTool.Pages.GamePages;
using WutheringWavesTool.Services.DialogServices;
using WutheringWavesTool.Services.Navigations.NavigationViewServices;
using WutheringWavesTool.ViewModel.GameViewModels;
using ZXing;
using ZXing.Common;
using WutheringWavesTool.Models.Wrapper;
namespace WutheringWavesTool.ViewModel;

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
        this.AppContext.App.MainWindow.Show(false);
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

    private async void LoginMessangerMethod(object recipient, LoginMessanger message)
    {
        this.LoginBthVisibility = Visibility.Collapsed;
        CommunitySelectItemVisiblity = Visibility.Visible;
        await RefreshRoleLists();
        await Task.Delay(800);
        this.AppContext.MainTitle.UpDate();
    }

    [RelayCommand]
    public async Task RefreshRoleLists()
    {
        var gamers = await WavesClient.GetWavesGamerAsync(this.CTS.Token);
        if (gamers == null || gamers.Code != 200)
            return;
        this.Roles = gamers.Data.FormatRoil();
        if(Roles != null)
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
            Environment.Exit(0);
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
        this.AppContext.MainTitle.UpDate();
        OpenMain();
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
    void OpenCounter(RoutedEventArgs args)
    {
        
    }

    private void ComputerWin_Closed(object sender, WindowEventArgs args)
    {
    }

    internal void SetSelectItem(Type sourcePageType)
    {
        var page = this.HomeNavigationViewService.GetSelectItem(sourcePageType);
        SelectItem = page;
    }
}
