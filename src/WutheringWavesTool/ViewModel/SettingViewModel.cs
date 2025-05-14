using H.NotifyIcon;
using Windows.ApplicationModel.DataTransfer;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel;

public sealed partial class SettingViewModel : ViewModelBase
{
    public SettingViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IWavesClient wavesClient,
        IAppContext<App> appContext,
        IViewFactorys viewFactorys
    )
    {
        DialogManager = dialogManager;
        WavesClient = wavesClient;
        AppContext = appContext;
        ViewFactorys = viewFactorys;
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
    }

    [RelayCommand]
    async Task CopyToken()
    {
        if (await WavesClient.IsLoginAsync())
        {
            DataPackage package = new();
            package.SetText(WavesClient.Token);
            Clipboard.SetContent(package);
        }
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
