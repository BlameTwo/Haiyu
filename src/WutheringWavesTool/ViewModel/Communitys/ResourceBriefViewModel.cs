

using System.Threading.Tasks;
using Waves.Api.Models.Communitys.DataCenter.ResourceBrief;

namespace WutheringWavesTool.ViewModel.Communitys;


public partial class ResourceBriefViewModel:ViewModelBase,IDisposable
{
    public ResourceBriefViewModel(IWavesClient wavesClient,ITipShow tipShow)
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
        if(value == "版本")
        {
            this.SubBrieItems = this.BrieVersion.ToObservableCollection();
        }
        else if(value == "月")
        {
            this.SubBrieItems = this.BrieMonth.ToObservableCollection();
        }
        else if(value == "周")
        {
            this.SubBrieItems = this.BrieWeek.ToObservableCollection();
        }
        this.SelectSubBrieItem = SubBrieItems[0];
    }

    async partial void OnSelectSubBrieItemChanged(BrieItem value)
    {
        if(value == null) return;
        if (SelectHeaderName == "版本")
        {
            var data = await WavesClient.GetVersionBrefItemAsync(this.Item.RoleId, this.Item.ServerId, value.Index.ToString(), this.CTS.Token);
        }
        else if (SelectHeaderName == "月")
        {
            var data = await WavesClient.GetMonthBrefItemAsync(this.Item.RoleId, this.Item.ServerId, value.Index.ToString(), this.CTS.Token);
        }
        else if (SelectHeaderName == "周")
        {
            var data = await WavesClient.GetWeekBrefItemAsync(this.Item.RoleId, this.Item.ServerId, value.Index.ToString(), this.CTS.Token);
        }
    }
    
    private async Task RefreshDataAsync()
    {
        var cache = await WavesClient.GetBriefHeaderAsync(this.CTS.Token);
        if(cache == null ||cache.Code != 200)
        {
            TipShow.ShowMessage("拉取数据失败", Symbol.Clear);
        }
        this.BrieWeek = cache.Data.Weeks;
        this.BrieVersion = cache.Data.Versions;
        this.BrieMonth = cache.Data.Months;
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

    public void Dispose()
    {
        this.CTS.Cancel();
        this.CTS.Dispose();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}
