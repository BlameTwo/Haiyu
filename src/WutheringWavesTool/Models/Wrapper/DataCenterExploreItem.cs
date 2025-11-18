namespace Haiyu.Models.Wrapper;

public partial class DataCenterExploreItem : ObservableObject
{
    [ObservableProperty]
    public partial ObservableCollection<DataCenterExploreCountryItem> Country { get; set; }

    [ObservableProperty]
    public partial string CountryName { get; set; }

    [ObservableProperty]
    public partial double CountryProgress { get; set; }

    [ObservableProperty]
    public partial BitmapImage Icon { get; set; }

    public DataCenterExploreItem(ExploreList item)
    {
        this.Country = new ObservableCollection<DataCenterExploreCountryItem>();
        foreach (var areainfo in item.AreaInfoList)
        {
            this.Country.Add(new DataCenterExploreCountryItem(areainfo));
        }
        this.CountryName = item.Country.CountryName;
        this.CountryProgress = Convert.ToDouble(item.CountryProgress);

        this.Icon = new(new(item.Country.HomePageIcon));
    }
}
