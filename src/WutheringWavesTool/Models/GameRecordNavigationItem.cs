namespace Haiyu.Models;

public class GameRecordNavigationItem
{
    public int Id { get; set; }

    public string DisplayName { get; set; }

    public static ObservableCollection<GameRecordNavigationItem> Default =>
         new ObservableCollection<GameRecordNavigationItem>()
        {
            new GameRecordNavigationItem() { Id = 1, DisplayName = "角色活动唤取" },
            new GameRecordNavigationItem() { Id = 2, DisplayName = "武器活动唤取" },
            new GameRecordNavigationItem() { Id = 3, DisplayName = "角色常驻唤取" },
            new GameRecordNavigationItem() { Id = 4, DisplayName = "武器常驻唤取" },
            new GameRecordNavigationItem() { Id = 5, DisplayName = "新手唤取" },
            new GameRecordNavigationItem() { Id = 6, DisplayName = "新手自选唤取" },
            new GameRecordNavigationItem() { Id = 7, DisplayName = "感恩定向唤取" },
            new GameRecordNavigationItem() { Id = 8, DisplayName = "角色新旅唤取" },
            new GameRecordNavigationItem() { Id = 9, DisplayName = "武器新旅唤取" },
        };
}
