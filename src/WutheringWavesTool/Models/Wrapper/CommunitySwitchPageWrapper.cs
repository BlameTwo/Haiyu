namespace WutheringWavesTool.Models.Wrapper;

public sealed partial class CommunitySwitchPageWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string DisplayName { get; set; }

    [ObservableProperty]
    public partial string Tag { get; set; }

    [ObservableProperty]
    public partial string Icon { get; set; }

    public static List<CommunitySwitchPageWrapper> GetDefault() =>
        new List<CommunitySwitchPageWrapper>()
        {
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "共鸣者",
                Tag = "DataGamer",
                Icon = "\uE6BB",
            },
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "数据坞",
                Tag = "DataDock",
                Icon = "\uE610",
            },
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "全息战略",
                Tag = "DataChallenge",
                Icon = "\uE6C6",
            },
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "逆境深塔",
                Tag = "DataAbyss",
                Icon = "\uE600",
            },
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "世界探索",
                Tag = "DataWorld",
                Icon = "\uE79F",
            },
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "图鉴收集",
                Tag = "Skin",
                Icon = "\uE62D",
            },
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "冥歌海域",
                Tag = "Boss2",
                Icon = "\uE6CA",
            },
            new CommunitySwitchPageWrapper()
            {
                DisplayName = "资源简报",
                Tag = "Resource",
                Icon ="\uE6B5"
            }
        };
}
