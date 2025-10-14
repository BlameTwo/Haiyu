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
        var result = await this.GameWikiClient.GetEventDataAsync(WikiType.Waves, this.CTS.Token);
        Sides = Format(result);
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
                TotalSpan = (DateTime.Parse(item.CountDown.DateRange[1]) - DateTime.Now).ToString(),
            };
            value.Cali();
            wrappers.Add(value);
        }
        return wrappers;
    }
}
