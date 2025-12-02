using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using Haiyu.Helpers;
using Haiyu.Models.Wrapper.Wiki;
using Waves.Api.Models.GameWikiiClient;

namespace Haiyu.ViewModel.WikiViewModels;

public partial class WavesWikiViewModel : WikiViewModelBase
{
    public WavesWikiViewModel()
    {
        this.Messenger.Register<LoginMessanger>(this, LoginMessangerMethod);
    }

    private async void LoginMessangerMethod(object recipient, LoginMessanger message)
    {
        await Loaded();
    }

    [ObservableProperty]
    public partial ObservableCollection<HotContentSideWrapper> Sides { get; set; } = [];

    [ObservableProperty]
    public partial bool Loading { get; set; }

    [ObservableProperty]
    public partial bool KuroLogin { get; set; } = false;

    [ObservableProperty]
    public partial ObservableCollection<StaminaWrapper> Staminas { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<WikiCatalogueChildren> CatalogueChildren { get; set; } = [];

    [RelayCommand]
    async Task Loaded()
    {
        Loading = true;
        var wikiPage = await TryInvokeAsync(async () =>
            await this.GameWikiClient.GetHomePageAsync(WikiType.Waves, this.CTS.Token)
        );
        if (await WavesClient.IsLoginAsync(CTS.Token))
        {
            var roles = await TryInvokeAsync(async () =>
                await WavesClient.GetGamerAsync( Waves.Core.Models.Enums.GameType.Waves,this.CTS.Token)
            );
            if (roles.Code != 0)
            {
                TipShow.ShowMessage($"获取数据失败，请检查网络或重启应用", Symbol.Clear);
                return;
            }
            foreach (var item in roles.Result.Data)
            {
                var stamina = await WavesClient.GetGamerBassDataAsync(item);
                if (stamina == null)
                    continue;
                this.Staminas.Add(new(stamina));
            }
            this.KuroLogin = true;
        }
        if (wikiPage.Code == 0 || (wikiPage.Result != null && wikiPage.Result.Data.ContentJson.Shortcuts != null))
        {
            Sides = GameWikiClient.GetEventData(wikiPage.Result).Format()??[];
            //var content = wikiPage.Result.Data.ContentJson.MainModules.Where(x => x.Type == "catalogue").FirstOrDefault().Content;
            //if(content is JsonElement contentElement)
            //{
            //    WikiCatalogue catalogue = contentElement.Deserialize(WikiContext.Default.WikiCatalogue);
            //    this.CatalogueChildren = catalogue.Childrens.ToObservableCollection();
            //}
        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络或重启应用", Symbol.Clear);
        }
        Loading = false;
    }

    public override void Dispose()
    {
        Sides.Clear();
        Staminas.Clear();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        base.Dispose();
    }
}
