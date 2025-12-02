using Waves.Core.Models.Enums;

namespace Haiyu.Models.Wrapper;

public partial class GameRoilDataWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string Id { get; set; }

    [ObservableProperty]
    public partial string RoleName { get; set; }

    [ObservableProperty]
    public string ServerName { get; }

    [ObservableProperty]
    public partial BitmapImage GameHeadUrl { get; set; }

    [ObservableProperty]
    public partial GameType Type { get; set; }

    [ObservableProperty]
    public partial int GameLevel { get; set; }


    public GameRoilDataItem Item { get; set; }

    public GameRoilDataWrapper(GameRoilDataItem item)
    {
        Item = item;
        this.Id = item.Id;
        this.RoleName = item.RoleName;
        this.ServerName = item.ServerName;
        if (item.HeadPhotoUrl == null)
        {
            this.GameHeadUrl = new BitmapImage(new("https://mc.kurogames.com/cloud/assets/avatar-cb06ab22.png"));
            return;
        }
        this.GameHeadUrl = new(new(item.HeadPhotoUrl));
    }
}


public static class GameRoilDataWrapperExtension
{
    public static ObservableCollection<GameRoilDataWrapper> FormatRoil(this List<GameRoilDataItem> roilDataItems, GameType type)
    {
        ObservableCollection<GameRoilDataWrapper> values = new();
        foreach (var item in roilDataItems)
        {
            GameRoilDataWrapper value = new GameRoilDataWrapper(item);
            value.Type = type;
            values.Add(value);
        }
        return values;
    }
  
}