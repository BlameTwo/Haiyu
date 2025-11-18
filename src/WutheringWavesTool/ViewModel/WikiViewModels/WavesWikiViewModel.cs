using Haiyu.Models.Wrapper.Wiki;
using Waves.Api.Models.GameWikiiClient;

namespace Haiyu.ViewModel.WikiViewModels;

public partial class WavesWikiViewModel : WikiViewModelBase
{
    public WavesWikiViewModel() { }

    [ObservableProperty]
    public partial ObservableCollection<HotContentSideWrapper> Sides { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        var result = await TryInvokeAsync(async () => await this.GameWikiClient.GetEventDataAsync(WikiType.Waves, this.CTS.Token));
        var result2 = await TryInvokeAsync(async () => await this.GameWikiClient.GetEventTabDataAsync(WikiType.Waves, this.CTS.Token));
        if (result.Item1 == 0)
        {
            Sides = Format(result.Item2);
        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络,{result.Item3}", Symbol.Clear);
        }
    }

    public ObservableCollection<HotContentSideWrapper> Format(IEnumerable<HotContentSide> result)
    {
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
