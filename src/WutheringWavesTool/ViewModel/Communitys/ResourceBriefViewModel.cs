using Waves.Api.Models.Communitys.DataCenter.ResourceBrief;

namespace Haiyu.ViewModel.Communitys;


public partial class ResourceBriefViewModel : ViewModelBase, IDisposable
{
    public ResourceBriefViewModel(IWavesClient wavesClient, ITipShow tipShow)
    {
        WavesClient = wavesClient;
        TipShow = tipShow;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    public IWavesClient WavesClient { get; }
    public ITipShow TipShow { get; }
    public GameRoilDataItem Item { get; private set; }

    [ObservableProperty]
    public partial ObservableCollection<string> HeanderName { get; set; } = new ObservableCollection<string>()
    {
        "版本",
        "月",
        "周"
    };

    [ObservableProperty]
    public partial ObservableCollection<BrefListItem> CoinList { get; set; }


    [ObservableProperty]
    public partial ObservableCollection<BrefListItem> StarList { get; set; }

    public List<BrieItem> BrieWeek { get; set; }


    public List<BrieItem> BrieMonth { get; set; }
    public List<BrieItem> BrieVersion { get; set; }

    [ObservableProperty]
    public partial string SelectHeaderName { get; set; }


    [ObservableProperty]
    public partial ObservableCollection<BrieItem> SubBrieItems { get; set; }


    [ObservableProperty]
    public partial BrieItem SelectSubBrieItem { get; set; }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        await RefreshDataAsync();
    }
    partial void OnSelectHeaderNameChanged(string value)
    {
        if (value == null)
            return;
        if (value == "版本")
        {
            this.SubBrieItems = this.BrieVersion.ToObservableCollection();
        }
        else if (value == "月")
        {
            this.SubBrieItems = this.BrieMonth.ToObservableCollection();
        }
        else if (value == "周")
        {
            this.SubBrieItems = this.BrieWeek.ToObservableCollection();
        }
        this.SelectSubBrieItem = SubBrieItems[0];
    }

    async partial void OnSelectSubBrieItemChanged(BrieItem value)
    {
        if (value == null) return;
        if (SelectHeaderName == "版本")
        {
            var data = await TryInvokeAsync(async () => await WavesClient.GetVersionBrefItemAsync(this.Item.RoleId, this.Item.ServerId, value.Index.ToString(), this.CTS.Token));
            if (data.Item1 != 0 || data.Item2.Code != 200)
            {
                TipShow.ShowMessage(data.Item2.Msg ?? "数据拉取错误", Symbol.Clear);
                return;
            }
            this.CoinList = data.Item2.Data.CoinList.ToObservableCollection();
            this.StarList = data.Item2.Data.StarList.ToObservableCollection();
        }
        else if (SelectHeaderName == "月")
        {
            var data = await WavesClient.GetMonthBrefItemAsync(this.Item.RoleId, this.Item.ServerId, value.Index.ToString(), this.CTS.Token);
            if (data == null || data.Code != 200)
            {
                TipShow.ShowMessage(data.Msg ?? "数据拉取错误", Symbol.Clear);
                return;
            }
            this.CoinList = data.Data.CoinList.ToObservableCollection();
            this.StarList = data.Data.StarList.ToObservableCollection();
        }
        else if (SelectHeaderName == "周")
        {
            var data = await WavesClient.GetWeekBrefItemAsync(this.Item.RoleId, this.Item.ServerId, value.Index.ToString(), this.CTS.Token);
            if (data == null || data.Code != 200)
            {
                TipShow.ShowMessage(data.Msg ?? "数据拉取错误", Symbol.Clear);
                return;
            }
            this.CoinList = data.Data.CoinList.ToObservableCollection();
            this.StarList = data.Data.StarList.ToObservableCollection();
        }
    }

    private async Task RefreshDataAsync()
    {
        var cache = await TryInvokeAsync(async () => await WavesClient.GetBriefHeaderAsync(this.CTS.Token));
        if (cache.Item1 != 0 || cache.Item2.Code != 200)
        {
            TipShow.ShowMessage("拉取数据失败", Symbol.Clear);
            return;
        }
        this.BrieWeek = cache.Item2.Data.Weeks;
        this.BrieVersion = cache.Item2.Data.Versions;
        this.BrieMonth = cache.Item2.Data.Months;
        this.SelectHeaderName = HeanderName[0];
    }

    internal void SetData(GameRoilDataItem item)
    {
        this.Item = item;
    }

    [RelayCommand]
    async Task Loaded()
    {
        await RefreshDataAsync();
    }

    public override void Dispose()
    {
        this.CTS.Cancel();
        this.CTS.Dispose();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}
