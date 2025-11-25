using Haiyu.Models.Wrapper.Wiki;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using Waves.Api.Models.GameWikiiClient;

namespace Haiyu.ViewModel.WikiViewModels;

public partial class WavesWikiViewModel : WikiViewModelBase
{
    public WavesWikiViewModel() { }

    [ObservableProperty]
    public partial ObservableCollection<HotContentSideWrapper> Sides { get; set; }


    [ObservableProperty]
    public partial ObservableCollection<StaminaWrapper> Staminas { get; set; } = [];

    [RelayCommand]
    async Task Loaded()
    {
        var result = await TryInvokeAsync(async () => await this.GameWikiClient.GetEventDataAsync(WikiType.Waves, this.CTS.Token));
        var result2 = await TryInvokeAsync(async () => await this.GameWikiClient.GetEventTabDataAsync(WikiType.Waves, this.CTS.Token));
        if(await this.WavesClient.IsLoginAsync(this.CTS.Token))
        {
            var userDatas = await TryInvokeAsync(async () => WavesClient.GetWavesGamerAsync(this.CTS.Token));
            var roles = await WavesClient.GetWavesGamerAsync(this.CTS.Token);
            if (roles == null || roles.Code != 200)
            {
                return;
            }
            foreach (var item in roles.Data)
            {
                var stamina = await WavesClient.GetGamerBassDataAsync(item);
                if (stamina == null)
                    continue;
                this.Staminas.Add(new(stamina));
            }
            this.IsLogin = true;
        }
        if (result.Item1 == 0)
        {
            Sides = Format(result.Item2);
        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络,{result.Item3}", Symbol.Clear);
        }
    }

    public ObservableCollection<HotContentSideWrapper> Format(IEnumerable<HotContentSide>? result)
    {
        if (result == null)
            return [];
        ObservableCollection<HotContentSideWrapper> wrappers = new();
        foreach (var item in result)
        {
            var value = new HotContentSideWrapper()
            {
                Title = item.Title,
                ImageUrl = item.ContentUrl,
                StartTime = item.CountDown.DateRange[0],
                EndTime = item.CountDown.DateRange[1],
            };
            var spanResult = (DateTime.Parse(item.CountDown.DateRange[1]) - DateTime.Now);
            value.Cali();
            wrappers.Add(value);
        }
        return wrappers;
    }
}
