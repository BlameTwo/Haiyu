using Waves.Api.Models.CloudGame;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel;

public partial class CloudGameViewModel : ViewModelBase
{
    public CloudGameViewModel(
        ICloudGameService cloudGameService,
        ITipShow tipShow,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager
    )
    {
        CloudGameService = cloudGameService;
        TipShow = tipShow;
        DialogManager = dialogManager;
    }

    public ICloudGameService CloudGameService { get; }
    public ITipShow TipShow { get; }
    public IDialogManager DialogManager { get; }

    [ObservableProperty]
    public partial ObservableCollection<LoginData> Users { get; set; }

    [ObservableProperty]
    public partial LoginData SelectedUser { get; set; }

    [ObservableProperty]
    public partial Visibility LoadVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility DataVisibility { get; set; } = Visibility.Collapsed;    

    [RelayCommand]
    public async Task Loaded()
    {
        // 请在当前代码中写入获取本地User的逻辑
        var users = await TryInvokeAsync(CloudGameService.ConfigManager.GetUsersAsync());
        if (users.Item1 != 0)
        {
            TipShow.ShowMessage("获取本地用户失败", Symbol.Clear);
        }
        this.Users = users.Item2;
    }

    async partial void OnSelectedUserChanged(LoginData value)
    {
        if (value == null)
            return;
        this.LoadVisibility = Visibility.Visible;
        this.DataVisibility = Visibility.Collapsed;
        var result = await  CloudGameService.OpenUserAsync(value);
        await Task.Delay(2000);
        this.LoadVisibility = Visibility.Collapsed;
        this.DataVisibility = Visibility.Visible;
    }


    [RelayCommand]
    public async Task ShowAdd()
    {
        await DialogManager.ShowWebGameDialogAsync();
    }

    [RelayCommand]
    public async Task GetRecordUrlAsync()
    {
        var url = await TryInvokeAsync(CloudGameService.GetRecordAsync());
        var resource = await TryInvokeAsync(CloudGameService.GetGameRecordResource(url.Item2.Data.RecordId,url.Item2.Data.PlayerId.ToString()));
    }
}
