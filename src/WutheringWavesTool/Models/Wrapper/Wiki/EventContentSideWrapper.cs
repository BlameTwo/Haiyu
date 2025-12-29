namespace Haiyu.Models.Wrapper.Wiki;

public partial class EventContentSideWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string Title { get; set; }

    [ObservableProperty]
    public partial string ImgMode { get; set; }

    [ObservableProperty]
    public partial DateTime StartTime { get; set; }


    [ObservableProperty]
    public partial DateTime StopTime { get; set; }

    /// <summary>
    /// 大图
    /// </summary>
    [ObservableProperty]
    public partial string BigImage { get; set; }

    /// <summary>
    /// 小图
    /// </summary>
    [ObservableProperty]
    public partial string Image1 { get; set; }


    [ObservableProperty]
    public partial string Image2 { get; set; }

    [ObservableProperty]
    public partial string Image3 { get; set; }

    [ObservableProperty]
    public partial string Image4 { get; set; }

    [ObservableProperty]
    public partial string DisplayTime { get; set; }

    [ObservableProperty]
    public partial double MaxProgress { get; set; }


    [ObservableProperty]
    public partial double CurrentProgress { get; set; }
    internal void Cali()
    {
        var time = this.StopTime - this.StartTime;
        var time2 = this.StopTime - DateTime.Now;
        this.MaxProgress = time.TotalSeconds;
        this.CurrentProgress = (this.StopTime - DateTime.Now).TotalSeconds;
        this.DisplayTime = $"{time2.Days}天{time2.Hours}小时{time2.Minutes}分{time2.Seconds}秒";
    }
}