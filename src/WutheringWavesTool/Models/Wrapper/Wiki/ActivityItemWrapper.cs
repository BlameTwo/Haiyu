using Waves.Api.Models.Wiki;

namespace WutheringWavesTool.Models.Wrapper.Wiki;

public sealed partial class ActivityWrapper:ObservableObject
{
    [ObservableProperty]
    public partial string Titlebar { get; set; }


    [ObservableProperty]
    public partial DateTime StartTime { get; set; }

    [ObservableProperty]
    public partial DateTime StopTime { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<SideImg> Images { get; set; }

    [ObservableProperty]
    public partial TimeSpan Time { get; set; }

}
