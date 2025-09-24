using Haiyu.Helpers;

namespace Haiyu.Models.Wrapper.WindowRoils;

public interface INavigationRoilsItem { }

public partial class NavigationRoilsDetilyItem : ObservableObject, INavigationRoilsItem
{
    public NavigationRoilsDetilyItem(RoleList roleData, GameRoilDataItem item)
    {
        this.StarLevel = roleData.StarLevel;
        this.RoilId = roleData.RoleId;
        this.AttributeName = roleData.AttributeName;
        this.RoleName = roleData.RoleName;
        this.AttibuteId = roleData.AttributeId;
        TypeImage = new BitmapImage(new(RoleHelper.SwitchType(roleData.AttributeId)));
        RoleIconUrl = new BitmapImage(new(roleData.RoleIconUrl));
        Item = item;
        
    }

    [ObservableProperty]
    public partial int StarLevel { get; set; }
    public long RoilId { get; }

    [ObservableProperty]
    public partial BitmapImage RoleIconUrl { get; set; }

    [ObservableProperty]
    public partial string AttributeName { get; set; }

    [ObservableProperty]
    public partial int AttibuteId { get; set; }

    [ObservableProperty]
    public partial BitmapImage TypeImage { get; set; }

    [ObservableProperty]
    public partial string RoleName { get; set; }
    public GameRoilDataItem Item { get; }
}

public partial class NavigationRoilsTypeItem : ObservableObject, INavigationRoilsItem
{
    [ObservableProperty]
    public partial string Name { get; set; }

    public NavigationRoilsTypeItem(string value)
    {
        this.Name = value;
    }
}
