using Waves.Api.Models.Wiki;

namespace WutheringWavesTool.ViewModel;

public sealed partial class ActivityTimeViewModel : ViewModelBase
{
    public ActivityTimeViewModel(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
    }

    public IWavesClient WavesClient { get; }

    [ObservableProperty]
    public partial EventsSide RuleSide { get; set; }

    [ObservableProperty]
    public partial EventsSide WeaponSide { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        var result = await this.TryInvokeAsync(WavesClient.GetMainWikiAsync(this.CTS.Token));
        if(result.Item1 != 0)
        {
            return;
        }
        foreach (var item in result.Item2.Data.ContentJson.SideModules)
        {
            try
            {
                if (item.Content is JsonElement element)
                {
                    if (item.Type == "events-side")
                    {
                        var side = element.Deserialize(WikiContext.Default.EventsSide);
                    }
                }
            }
            catch (Exception)
            {
                continue;
            }
            
        }
    }
}
