namespace WutheringWavesTool.ViewModel;

public sealed partial class ToolViewModel : ViewModelBase
{
    public ToolViewModel(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
    }

    public IWavesClient WavesClient { get; }

    [ObservableProperty]
    public partial bool IsLogin { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<StaminaWrapper> Staminas { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        await RefreshAsync();
    }

    [RelayCommand]
    async Task RefreshAsync()
    {
        this.IsLogin = await WavesClient.IsLoginAsync(this.CTS.Token);
        if (!IsLogin)
        {
            IsLogin = false;
            return;
        }
        var roles = await WavesClient.GetWavesGamerAsync(this.CTS.Token);
        if (roles == null || roles.Code != 200)
        {
            IsLogin = false;
            return;
        }
        this.IsLogin = true;
        Staminas = new();
        foreach (var item in roles.Data)
        {
            var result = await WavesClient.GetGamerBassDataAsync(item);
            if (result == null)
                continue;
            this.Staminas.Add(new(result));
        }
    }
}
