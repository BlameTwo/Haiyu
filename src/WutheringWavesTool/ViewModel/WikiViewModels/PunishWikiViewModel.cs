
using Haiyu.Helpers;
using Haiyu.Models.Wrapper.Wiki;
using Waves.Api.Models.GameWikiiClient;

namespace Haiyu.ViewModel.WikiViewModels;

public partial class PunishWikiViewModel : WikiViewModelBase
{
    public PunishWikiViewModel() { }

    [ObservableProperty]
    public partial ObservableCollection<HotContentSideWrapper> Sides { get; set; }

    [ObservableProperty]
    public partial PunishEveryWeekContent EveryWeekContent { get; set; } = new();

    [RelayCommand]
    async Task Loaded()
    {
        var wikiPage = await TryInvokeAsync(async () =>
            await this.GameWikiClient.GetHomePageAsync(WikiType.BGR, this.CTS.Token)
        );
        if (wikiPage.Code == 0 || (wikiPage.Result != null && wikiPage.Result.Data.ContentJson.Shortcuts != null))
        {
            Sides = GameWikiClient.GetEventData(wikiPage.Result).Format() ?? [];

            var normanModule = wikiPage.Result.Data.ContentJson.SideModules;
            var mainModule = wikiPage.Result.Data.ContentJson.MainModules;
            EveryWeekContent.InitWeekContent(normanModule, mainModule);
            EveryWeekContent.UpdatePunishCageContent();

        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络或重启应用", Symbol.Clear);
        }
    }

    private void TestFunction((int code, WikiHomeModel result, string? msg) wikiPage)
    {
        using (StreamWriter sw = new StreamWriter("F:\\Code_Project\\haiyu\\Dev\\Txt.txt"))
        {
            foreach (var value in wikiPage.result.Data.ContentJson.SideModules)
            {
                sw.WriteLine(value.Title);
                sw.WriteLine(value.Content);
                sw.WriteLine("\n");
            }
            sw.WriteLine("\n\n\n");
            sw.WriteLine("MainModule:\n");
            foreach (var value in wikiPage.result.Data.ContentJson.MainModules)
            {
                sw.WriteLine(value.Content);
            }
        }
    }
    public override void Dispose()
    {
        Sides.Clear();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        base.Dispose();
    }
}

