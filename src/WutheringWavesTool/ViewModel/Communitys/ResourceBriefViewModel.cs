

using System.Threading.Tasks;

namespace WutheringWavesTool.ViewModel.Communitys;

public partial class ResourceBriefViewModel:ViewModelBase,IDisposable
{
    public ResourceBriefViewModel(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    public IWavesClient WavesClient { get; }
    public GameRoilDataItem Item { get; private set; }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        await RefreshDataAsync();
    }

    private async Task RefreshDataAsync()
    {
        var header = await WavesClient.GetBriefHeaderAsync(this.CTS.Token);
    }

    internal void SetData(GameRoilDataItem item)
    {
        this.Item = item;
    }

    [RelayCommand]
    async Task Loaded()
    {
        await RefreshDataAsync();
    }

    public void Dispose()
    {
        this.CTS.Cancel();
        this.CTS.Dispose();

        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}
