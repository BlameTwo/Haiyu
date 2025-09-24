namespace Haiyu.Models.Wrapper;

public sealed partial class DataCenterGamerChallengeIndexListWrapper : ObservableObject
{
    [ObservableProperty]
    public partial ObservableCollection<ChallengeRole> Roles { get; set; }

    [ObservableProperty]
    public partial int Time { get; set; }

    [ObservableProperty]
    public partial int BossLevel { get; set; }

    [ObservableProperty]
    public partial int Value { get; set; }

    [ObservableProperty]
    public partial bool ItemsVisibility { get; set; }

    public DataCenterGamerChallengeIndexListWrapper(Detilys detily)
    {
        if (detily.Roles == null)
        {
            this.ItemsVisibility = false;
        }
        else
        {
            ItemsVisibility = true;
            this.Roles = new(detily.Roles);
        }
        this.Time = detily.PassTime;
        this.BossLevel = detily.BossLevel;
        this.Value = detily.Difficulty;
    }
}

public sealed partial class DataCenterChallengeBossItemWrapper : ObservableObject
{
    public DataCenterChallengeBossItemWrapper(
        List<DataCenterGamerChallengeIndexListWrapper> wrappers,
        IGrouping<string, Detilys> detilys
    )
    {
        this.IndexWrapper = new(wrappers);
        this.BossCover = new(new(detilys.First().BossHeadIcon));
        BossHeader = new(new(detilys.First().BossIconUrl));
        this.BossName = detilys.First().BossName;
        this.MaxCount = wrappers.Count;
        this.Count = wrappers.Count(x => x.Roles != null);
    }

    [ObservableProperty]
    public partial BitmapImage BossCover { get; set; }

    [ObservableProperty]
    public partial BitmapImage BossHeader { get; set; }

    [ObservableProperty]
    public partial int MaxCount { get; set; }

    [ObservableProperty]
    public partial int Count { get; set; }

    [ObservableProperty]
    public partial string BossName { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterGamerChallengeIndexListWrapper> IndexWrapper { get; set; }
}

public sealed partial class DataCenterGamerChallengeCountryWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string CountryName { get; set; }

    [ObservableProperty]
    public partial int CountryId { get; set; }

    [ObservableProperty]
    public partial BitmapImage Cover { get; set; }

    public List<int> BossIds { get; private set; } = new();

    public DataCenterGamerChallengeCountryWrapper(ChallengeList country)
    {
        this.CountryName = country.Country.CountryName;
        this.BossIds = country.IndexList.Select(x => x.BossId).ToList();
        this.CountryId = country.Country.CountryId;
        this.Cover = new BitmapImage(new(country.Country.HomePageIcon));
    }
}
