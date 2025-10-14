using Waves.Api.Models.GameWikiiClient;

namespace Haiyu.ViewModel.WikiViewModels;

public partial class WavesWikiViewModel : WikiViewModelBase
{
    public WavesWikiViewModel() { }

    [ObservableProperty]
    public partial ObservableCollection<SideEventDataWrapper> Sides { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        var result = await TryInvokeAsync(async()=> await this.GameWikiClient.GetEventDataAsync(WikiType.Waves, this.CTS.Token));
        if(result.Item1 == 0)
        {
            Sides = Format(result.Item2);
        }
        else
        {
            TipShow.ShowMessage($"获取数据失败，请检查网络,{result.Item3}", Symbol.Clear);
        }
    }

    public ObservableCollection<SideEventDataWrapper> Format(IEnumerable<SideEventData> result)
    {
        ObservableCollection<SideEventDataWrapper> wrappers = new();
        foreach (var item in result)
        {
            var value = new SideEventDataWrapper()
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
