namespace Haiyu.Models.Wrapper;

public partial class DataCenterPhantomItemWrapper : ObservableObject
{
    public DataCenterPhantomItemWrapper(PhantomList bassData)
    {
        this.Star = bassData.Star;
        this.MaxStar = bassData.MaxStar;
        this.Name = bassData.Phantom.Name;
        this.CoverUrl = bassData.Phantom.IconUrl;
        this.Cover = new BitmapImage(new(CoverUrl));
    }

    [ObservableProperty]
    public partial int Star { get; set; }

    [ObservableProperty]
    public partial int MaxStar { get; set; }

    [ObservableProperty]
    public partial string Name { get; set; }
    public string CoverUrl { get; }

    [ObservableProperty]
    public partial BitmapImage Cover { get; set; }
}
