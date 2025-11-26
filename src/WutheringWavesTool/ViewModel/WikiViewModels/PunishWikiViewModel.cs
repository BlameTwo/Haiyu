
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
        var wikiPage = await TryInvokeAsync(async () =>
            await this.GameWikiClient.GetHomePageAsync(WikiType.Waves, this.CTS.Token)
        );
        if (wikiPage.Code == 0 || (wikiPage.Result != null && wikiPage.Result.Data.ContentJson.Shortcuts != null))
        {
            Sides = GameWikiClient.GetEventData(wikiPage.Result).Format() ?? [];
        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络或重启应用", Symbol.Clear);
        }
    }

    
    public override void Dispose()
    {
        Sides.Clear();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        base.Dispose();
    }
}

