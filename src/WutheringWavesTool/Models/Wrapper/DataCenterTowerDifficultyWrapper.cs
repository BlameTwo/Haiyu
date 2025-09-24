namespace Haiyu.Models.Wrapper;

public partial class DataCenterTowerDifficultyWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string DisplayName { get; set; }

    [ObservableProperty]
    public partial int Id { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterTowerAreaWrapper> Areas { get; set; }

    public DataCenterTowerDifficultyWrapper(DifficultyList item)
    {
        this.DisplayName = item.DifficultyName;
        this.Id = item.Difficulty;
        this.Areas = new ObservableCollection<DataCenterTowerAreaWrapper>(
            item.TowerAreaList.Select(x => new DataCenterTowerAreaWrapper(x))
        );
    }
}

public partial class DataCenterTowerAreaWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial int MaxStar { get; set; }

    [ObservableProperty]
    public partial int Star { get; set; }

    [ObservableProperty]
    public partial int AreaId { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterTowerFloorWrapper> Floors { get; set; }

    public DataCenterTowerAreaWrapper(TowerAreaList item)
    {
        this.Name = item.AreaName;
        this.MaxStar = item.MaxStar;
        this.Star = item.Star;
        this.Floors = new ObservableCollection<DataCenterTowerFloorWrapper>(
            item.FloorList.Select(x => new DataCenterTowerFloorWrapper(x))
        );
    }
}

public partial class DataCenterTowerFloorWrapper : ObservableObject
{
    [ObservableProperty]
    public partial int Floor { get; set; }

    [ObservableProperty]
    public partial BitmapImage PicUrl { get; set; }

    [ObservableProperty]
    public partial int Star { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterRoleWrapper> Roles { get; set; }

    public DataCenterTowerFloorWrapper(FloorList item)
    {
        this.Floor = item.Floor;
        this.Star = item.Star;
        this.PicUrl = new(new(item.PicUrl));
        if (item.RoleList == null)
            return;
        this.Roles = new ObservableCollection<DataCenterRoleWrapper>(
            item.RoleList.Select(x => new DataCenterRoleWrapper(x))
        );
    }
}

public partial class DataCenterRoleWrapper : ObservableObject
{
    public DataCenterRoleWrapper(TowerRoleList item)
    {
        this.Icon = new(new(item.IconUrl));
    }

    [ObservableProperty]
    public partial BitmapImage Icon { get; set; }
}
