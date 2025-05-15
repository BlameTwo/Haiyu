using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WutheringWavesTool.ViewModel.Communitys;

public sealed partial class GamerSlashDetailViewModel : ViewModelBase
{
    public GamerSlashDetailViewModel(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        await this.RefreshData();
    }

    public IWavesClient WavesClient { get; }
    public GameRoilDataItem Roil { get; private set; }

    public void SetData(GameRoilDataItem item)
    {
        this.Roil = item;
    }

    internal void Dispose()
    {
        this.CTS.Cancel();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    [RelayCommand]
    async Task Loaded()
    {
        await RefreshData();
    }

    async Task RefreshData()
    {
        var result = await WavesClient.GetGamerSlashDetailAsync(this.Roil, this.CTS.Token);
    }
}
