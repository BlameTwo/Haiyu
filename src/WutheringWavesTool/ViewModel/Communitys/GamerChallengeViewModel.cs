namespace WutheringWavesTool.ViewModel.Communitys;

public sealed partial class GamerChallengeViewModel : ViewModelBase, IDisposable
{
    private List<ChallengeList>? orginCountrys;

    public GamerChallengeViewModel(IWavesClient wavesClient, ITipShow tipShow)
    {
        Countrys = new();
        WavesClient = wavesClient;
        TipShow = tipShow;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        this.RoilItem = message.Data.Item;
        await Loaded();
    }

    public IWavesClient WavesClient { get; }
    public ITipShow TipShow { get; }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterGamerChallengeCountryWrapper> Countrys { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterChallengeBossItemWrapper> Items { get; set; }

    [ObservableProperty]
    public partial DataCenterGamerChallengeCountryWrapper SelectCountry { get; set; }

    public GameRoilDataItem RoilItem { get; private set; }

    async partial void OnSelectCountryChanged(DataCenterGamerChallengeCountryWrapper value)
    {
        if (value == null)
            return;
        if (Items != null && Items.Count > 0)
            Items.Clear();
        else
            Items = new();
        var result = await WavesClient.GetGamerChallengeDetails(
            this.RoilItem,
            value.CountryId,
            this.CTS.Token
        );
        if (result == null)
        {
            TipShow.ShowMessage("数据拉取失败！", Microsoft.UI.Xaml.Controls.Symbol.Clear);
            return;
        }
        var value2 = result!.ChallengeInfo.Detilys.GroupBy(x => x.BossId);
        foreach (var item in value2)
        {
            foreach (var com in value.BossIds)
            {
                if (int.Parse(item.Key) == com)
                {
                    List<DataCenterGamerChallengeIndexListWrapper> indexList =
                        new List<DataCenterGamerChallengeIndexListWrapper>();
                    foreach (var index in item)
                    {
                        indexList.Add(new(index));
                    }
                    DataCenterChallengeBossItemWrapper listItem =
                        new DataCenterChallengeBossItemWrapper(indexList, item);
                    if (Items == null)
                        Items = new();
                    this.Items.Add(listItem);
                }
            }
        }
    }

    [RelayCommand]
    async Task Loaded()
    {
        var challData = await WavesClient.GetGamerChallengeIndexDataAsync(RoilItem, this.CTS.Token);
        if (challData == null || challData.ChallengeList == null)
        {
            TipShow.ShowMessage("拉取数据为空", Symbol.Clear);
            return;
        }
        foreach (var item in challData.ChallengeList)
        {
            Countrys.Add(new(item));
        }
        if (Countrys.Count > 0)
        {
            SelectCountry = Countrys[0];
        }
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        this.Countrys.RemoveAll();
        this.orginCountrys?.Clear();
        if(Items != null)
        {
            foreach (var item in Items)
            {
                item.IndexWrapper?.RemoveAll();
                item.IndexWrapper = null;
                item.BossCover = null;
            }
        }
        this.Items.RemoveAll();
        this.Countrys = null;
        this.orginCountrys = null;
        this.Items = null;
    }

    internal async Task SetDataAsync(GameRoilDataItem Roilitem)
    {
        this.RoilItem = Roilitem;
    }
}
