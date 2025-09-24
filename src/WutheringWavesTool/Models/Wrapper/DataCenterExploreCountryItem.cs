namespace Haiyu.Models.Wrapper;

public partial class DataCenterExploreCountryItem : ObservableObject
{
    [ObservableProperty]
    public partial double Progress { get; set; }

    [ObservableProperty]
    public partial string DisplayName { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<ItemList> Items { get; set; }

    [ObservableProperty]
    public partial string AreaPic { get; set; }

    public DataCenterExploreCountryItem(AreaInfoList areaInfoList)
    {
        this.Progress = areaInfoList.AreaProgress;
        this.DisplayName = areaInfoList.AreaName;
        this.Items = areaInfoList.ItemList.ToObservableCollection();
        this.AreaPic = areaInfoList.AreaPic;
    }
}
