using Haiyu.Helpers;
using Haiyu.Services.DialogServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Security.Credentials.UI;

namespace Haiyu.ViewModel;

public sealed partial class SettingViewModel : ViewModelBase
{
    public SettingViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IWavesClient wavesClient,
        IAppContext<App> appContext,
        IViewFactorys viewFactorys,
        ITipShow tipShow,
        IScreenCaptureService screenCaptureService
    )
    {
        DialogManager = dialogManager;
        WavesClient = wavesClient;
        AppContext = appContext;
        ViewFactorys = viewFactorys;
        TipShow = tipShow;
        ScreenCaptureService = screenCaptureService;
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
    public IWavesClient WavesClient { get; }
    public IAppContext<App> AppContext { get; }
    public IViewFactorys ViewFactorys { get; }
    public ITipShow TipShow { get; }
    public IScreenCaptureService ScreenCaptureService { get; }

    [ObservableProperty]
    public partial ObservableCollection<GameRoilDataItem> GamerData { get; set; }

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
        this.InitCapture();
        GetAllVersion();
    }

    [RelayCommand]
    async Task CopyToken()
    {
        var result = await UserConsentVerifier.RequestVerificationAsync("复制授权码需要系统用户密码");
        if(result != UserConsentVerificationResult.Verified)
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
}
