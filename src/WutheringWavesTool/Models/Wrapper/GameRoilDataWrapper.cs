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
        this.GameHeadUrl = new(new(item.GameHeadUrl));
    }
}
