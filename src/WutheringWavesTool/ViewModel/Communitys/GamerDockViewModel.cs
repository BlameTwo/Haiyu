namespace Haiyu.ViewModel.Communitys;

public partial class GamerDockViewModel : ViewModelBase, IDisposable
{
    private bool disposedValue;

    public IWavesClient WavesClient { get; }
    public ITipShow TipShow { get; }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterPhantomItemWrapper> GamerPhantoms { get; set; }

    [ObservableProperty]
    public partial GamerCalabashData GamerCalabash { get; set; }
    public GameRoilDataItem GameRoil { get; private set; }

    public GamerDockViewModel(IWavesClient wavesClient, ITipShow tipShow)
    {
        WavesClient = wavesClient;
        TipShow = tipShow;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        await RefreshDataAsync(message.Data.Item);
    }

    private async Task RefreshDataAsync(GameRoilDataItem item)
    {
        this.GameRoil = item;
        var calabash = await TryInvokeAsync(async()=>await WavesClient.GetGamerCalabashDataAsync(GameRoil));
        if (calabash.Item1 == -2 || calabash.Item1 == -1)
        {
            TipShow.ShowMessage("未请求到数据坞信息", Microsoft.UI.Xaml.Controls.Symbol.Clear);
            TipShow.ShowMessage("拉取数据为空", Symbol.Clear);
        }
        else if(calabash.Item1 == 0)
        {
            this.GamerCalabash = calabash.Item2;
            this.GamerPhantoms = FormatData(calabash.Item2);
        }
    }

    private ObservableCollection<DataCenterPhantomItemWrapper> FormatData(
        GamerCalabashData calabash
    )
    {
        if (calabash.PhantomList == null)
            return [];
        ObservableCollection<DataCenterPhantomItemWrapper> items = new();
        foreach (var item in calabash.PhantomList)
        {
            items.Add(new(item));
        }
        return items;
    }

    public async Task SetDataAsync(GameRoilDataItem item)
    {
        await RefreshDataAsync(item);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                WeakReferenceMessenger.Default.UnregisterAll(this);
                GamerPhantoms.RemoveAll();
                GamerCalabash = null;
                GameRoil = null;
                this.CTS.Cancel();
                this.Messenger.UnregisterAll(this);
            }
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~GamerDockViewModel()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
