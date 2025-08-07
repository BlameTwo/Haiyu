namespace WutheringWavesTool.Models.Wrapper;

public partial class GameRoilDataWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string Id { get; set; }

    [ObservableProperty]
    public partial string RoleName { get; set; }

    [ObservableProperty]
    public partial BitmapImage GameHeadUrl { get; set; }

    [ObservableProperty]
    public partial int GameLevel { get; set; }

    public GameRoilDataItem Item { get; set; }

    public GameRoilDataWrapper(GameRoilDataItem item)
    {
        Item = item;
        this.Id = item.Id;
        this.RoleName = item.RoleName;
        this.GameHeadUrl = new(new(item.HeadPhotoUrl));
    }
}


public static class GameRoilDataWrapperExtension
{
    public static ObservableCollection<GameRoilDataWrapper> FormatRoil(this List<GameRoilDataItem> roilDataItems)
    {
        ObservableCollection<GameRoilDataWrapper> values = new();
        foreach (var item in roilDataItems)
        {
            GameRoilDataWrapper value = new GameRoilDataWrapper(item);
            values.Add(value);
        }
        return values;
    }
}