namespace WutheringWavesTool.Models.Wrapper.CommunitySlash;

public partial class SlashHeaderWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string SlashHeaderName { get; set; }

    [ObservableProperty]
    public partial string SlashHeaderIcon { get; set; }

    [ObservableProperty]
    public partial string SlashHeaderBG { get; set; }

    [ObservableProperty]
    public partial int Score { get; set; }

    [ObservableProperty]
    public partial int MaxScore { get; set; }

    public static SlashHeaderWrapper Convert(SlashDifficultyList value)
    {
        return new()
        {
            SlashHeaderName = value.DifficultyName,
            MaxScore = value.MaxScore,
            Score = value.AllScore,
            SlashHeaderBG = value.DetailPageBG,
            SlashHeaderIcon = value.TeamIcon,
        };
    }
}
