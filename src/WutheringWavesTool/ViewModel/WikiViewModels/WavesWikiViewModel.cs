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
    public partial ObservableCollection<HotContentSideWrapper> Actives { get; set; } = [];

    [ObservableProperty]
    public partial bool Loading { get; set; }

    [ObservableProperty]
    public partial bool KuroLogin { get; set; } = false;

    [ObservableProperty]
    public partial ObservableCollection<StaminaWrapper> Staminas { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<EventContentSideWrapper>? RoleActive { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<EventContentSideWrapper>? WeaponActive { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<WikiCatalogueChildren> CatalogueChildren { get; set; } = [];

    [RelayCommand]
    async Task Loaded()
    {
        Loading = true;
        var wikiPage = await TryInvokeAsync(async () =>
            await this.GameWikiClient.GetHomePageAsync(WikiType.Waves, this.CTS.Token)
        );
        await RefreshUserAsync();
        if (wikiPage.Code == 0 || (wikiPage.Result != null && wikiPage.Result.Data.ContentJson.Shortcuts != null))
        {
            Actives = GameWikiClient.GetEventData(wikiPage.Result)!.Format()??[];
            var sides = wikiPage.Result.Data.ContentJson.SideModules.Where(x => x.Type == "events-side").ToList();
            if(sides.Count == 2)
            {
                var role =  await FormatSideDataAsync(sides[0]);
                RoleActive = role?.ToObservableCollection();
                var weapon =  await FormatSideDataAsync(sides[1]);
                WeaponActive = weapon?.ToObservableCollection();
            }
            else
            {
                TipShow.ShowMessage("获取卡池信息出现了不可预料的情况，请确认官方Wiki显示是否正常", Symbol.Clear);
            }

        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络或重启应用", Symbol.Clear);
        }
        Loading = false;
    }

    private async Task<List<EventContentSideWrapper>?> FormatSideDataAsync(SideModule sideModules)
    {
        if (sideModules.Content is JsonElement jsonElement)
        {
            var jsonObject = jsonElement.Deserialize<EventContentSide>(WikiContext.Default.EventContentSide);
            List<EventContentSideWrapper> wrappers = new();
            foreach (var tag in jsonObject!.Tabs)
            {
                EventContentSideWrapper wrapper = new();
                wrapper.Title = tag.Name;
                wrapper.ImgMode = tag.ImgMode;
                if (DateTime.TryParse(tag.CountDown.DateRange[0], out var time) && DateTime.TryParse(tag.CountDown.DateRange[1], out var endTime))
                {
                    wrapper.StartTime = time;
                    wrapper.StopTime = endTime;
                }
                wrapper.Image1 = tag.Images[0].Image;
                wrapper.Image2 = tag.Images[1].Image;
                wrapper.Image3 = tag.Images[2].Image;
                wrapper.Image4 = tag.Images[3].Image;
                wrapper.Cali();
                wrappers.Add(wrapper);
            }
            return wrappers;
        }
        else
            return [];
    }

    [RelayCommand]
    private async Task RefreshUserAsync()
    {
        try
        {
            if (await WavesClient.IsLoginAsync(CTS.Token))
            {
                var roles = await TryInvokeAsync(async () =>
                    await WavesClient.GetGamerAsync(Waves.Core.Models.Enums.GameType.Waves, this.CTS.Token)
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
                TipShow.ShowMessage("刷新完成", Symbol.Accept);
            }
        }
        catch (Exception ex)
        {

            TipShow.ShowMessage($"刷新失败:{ex.Message}", Symbol.Accept);
        }
    }

    public override void Dispose()
    {
        Actives.Clear();
        Staminas.Clear();
        WeaponActive.Clear();
        RoleActive.Clear();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        base.Dispose();
    }
}
