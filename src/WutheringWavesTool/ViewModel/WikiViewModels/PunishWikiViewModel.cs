
using Haiyu.Helpers;
using Haiyu.Models.Wrapper.Wiki;
using Waves.Api.Models.GameWikiiClient;

namespace Haiyu.ViewModel.WikiViewModels;

public partial class PunishWikiViewModel : WikiViewModelBase
{
    public PunishWikiViewModel() { }

    [ObservableProperty]
    public partial ObservableCollection<HotContentSideWrapper> Sides { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        var result = await TryInvokeAsync(async () => await this.GameWikiClient.GetEventDataAsync(WikiType.BGR, this.CTS.Token));
        if (result.Item1 == 0)
        {
            Sides = result.Item2.Format();
        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络,{result.Item3}", Symbol.Clear);
        }
    }

    
    public override void Dispose()
    {
        Sides.Clear();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        base.Dispose();
    }
}

