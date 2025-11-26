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
    public partial ObservableCollection<StaminaWrapper> Staminas { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<WavesShortcutsWrapper> Sortcuts { get; set; } = [];

    [RelayCommand]
    async Task Loaded()
    {
        var result = await TryInvokeAsync(async () =>
            await this.GameWikiClient.GetEventDataAsync(WikiType.Waves, this.CTS.Token)
        );
        var wikiPage = await TryInvokeAsync(async () =>
            await this.GameWikiClient.GetHomePageAsync(WikiType.Waves, this.CTS.Token)
        );
        if (result.Item1 == 0)
        {
            Sides = result.Item2.Format();
        }
        if (await WavesClient.IsLoginAsync(CTS.Token))
        {
            var roles = await TryInvokeAsync(async () =>
                await WavesClient.GetWavesGamerAsync(this.CTS.Token)
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
        }
        if (wikiPage.Code == 0 || (wikiPage.Result != null && wikiPage.Result.Data.ContentJson.Shortcuts != null))
        {
            foreach (var item in wikiPage.Result.Data.ContentJson.Shortcuts.Content)
            {
                if (item.LinkConfig.CatalogueId is JsonElement element)
                {
                    Sortcuts.Add(
                        new WavesShortcutsWrapper()
                        {
                            Title = item.Title,
                            ContentUrl = item.ContentUrl,
                            CatalogueId = element.GetInt32().ToString(),
                            LinkType = item.LinkConfig.LinkType.ToString(),
                        }
                    );
                }
            }
        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络或重启应用", Symbol.Clear);
        }
        this.IsLogin = true;
    }

    public override void Dispose()
    {
        Sides.Clear();
        Staminas.Clear();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        base.Dispose();
    }
}
