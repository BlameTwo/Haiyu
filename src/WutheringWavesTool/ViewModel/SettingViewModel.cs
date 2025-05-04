using Windows.ApplicationModel.DataTransfer;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel;

public sealed partial class SettingViewModel : ViewModelBase
{
    public SettingViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IWavesClient wavesClient,
        IAppContext<App> appContext
    )
    {
        DialogManager = dialogManager;
        WavesClient = wavesClient;
        AppContext = appContext;
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

    [ObservableProperty]
    public partial ObservableCollection<GameRoilDataItem> GamerData { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        if (await WavesClient.IsLoginAsync())
        {
            var gamers = await WavesClient.GetWavesGamerAsync(this.CTS.Token);
            if (gamers != null)
                this.GamerData = gamers.Data.ToObservableCollection();
        }
        this.SelectTheme = AppSettings.AppTheme ?? "Default";
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
}
