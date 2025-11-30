using Haiyu.Helpers;
using Haiyu.Services.DialogServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Security.Credentials.UI;

namespace Haiyu.ViewModel;

public sealed partial class SettingViewModel : ViewModelBase
{
    public SettingViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IKuroClient wavesClient,
        IAppContext<App> appContext,
        IViewFactorys viewFactorys,
        ITipShow tipShow,
        IScreenCaptureService screenCaptureService,
        IPickersService pickersService
    )
    {
        DialogManager = dialogManager;
        WavesClient = wavesClient;
        AppContext = appContext;
        ViewFactorys = viewFactorys;
        TipShow = tipShow;
        ScreenCaptureService = screenCaptureService;
        PickersService = pickersService;
        RegisterMessanger();
    }

    private void RegisterMessanger()
    {
        this.Messenger.Register<CopyStringMessager>(this, CopyString);
    }

    private void CopyString(object recipient, CopyStringMessager message)
    {
        var package = new DataPackage();
        package.SetText(message.Value);
        Clipboard.SetContent(package);
    }

    public IDialogManager DialogManager { get; }
    public IKuroClient WavesClient { get; }
    public IAppContext<App> AppContext { get; }
    public IViewFactorys ViewFactorys { get; }
    public ITipShow TipShow { get; }
    public IScreenCaptureService ScreenCaptureService { get; }
    public IPickersService PickersService { get; }
    [ObservableProperty]
    public partial ObservableCollection<GameRoilDataItem> GamerData { get; set; }

    [ObservableProperty]
    public partial bool? AutoCommunitySign { get; set; }

    [ObservableProperty]
    public partial int SelectCloseIndex { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        if (await WavesClient.IsLoginAsync())
        {
            var gamers = await WavesClient.GetWavesGamerAsync(this.CTS.Token);
            if (gamers != null)
                this.GamerData = gamers.Data.ToObservableCollection();
        }
        var closeWindow = AppSettings.CloseWindow;
        switch (closeWindow)
        {
            case "True":
                this.SelectCloseIndex = 1;
                break;
            case "False":
                this.SelectCloseIndex = 0;
                break;
        }
        this.OldCursorName = AppSettings.SelectCursor ?? "默认";
        this.SelectCursorName = Cursors.Find(x => x == AppSettings.SelectCursor) ?? Cursors[0];
        if (AppSettings.WallpaperType == null)
        {
            this.SelectWallpaperName = WallpaperTypes[0];
        }
        else
        {
            if (AppSettings.WallpaperType == "Video")
            {
                this.SelectWallpaperName = WallpaperTypes[0];
            }
            else
            {
                this.SelectWallpaperName = WallpaperTypes[1];
            }
        }
        this.InitCapture();
        GetAllVersion();
        if(bool.TryParse(AppSettings.AutoSignCommunity,out var isSign))
        {
            this.AutoCommunitySign = isSign;
        }
        else
        {
            this.AutoCommunitySign = false;
        }
    }

    [RelayCommand]
    async Task CopyToken()
    {
        var result = await UserConsentVerifier.RequestVerificationAsync(
            "复制授权码需要系统用户密码"
        );
        if (result != UserConsentVerificationResult.Verified)
        {
            TipShow.ShowMessage("系统用户验证失败！", Symbol.Clear);
            return;
        }
        if (await WavesClient.IsLoginAsync())
        {
            DataPackage package = new();
            package.SetText(WavesClient.Token);
            Clipboard.SetContent(package);
        }
    }

    [RelayCommand]
    async Task CopyDid()
    {
        DataPackage package = new();
        package.SetText(HardwareIdGenerator.GenerateUniqueId());
        Clipboard.SetContent(package);
    }

    [RelayCommand]
    void OpenDesktopTool()
    {
        ViewFactorys.ShowToolWindow();
    }

    partial void OnSelectCloseIndexChanged(int value)
    {
        switch (value)
        {
            case 0:
                AppSettings.CloseWindow = "False";
                break;
            case 1:
                AppSettings.CloseWindow = "True";
                break;
        }
    }

    partial void OnAutoCommunitySignChanged(bool? value)
    {
        if (value == null)
            return;
        AppSettings.AutoSignCommunity = value.ToString();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
