namespace Haiyu.ViewModel.Communitys;

public sealed partial class GameRoilsViewModel : ViewModelBase, ICommunityViewModel
{
    private bool disposedValue;
    private ObservableCollection<DataCenterRoilItemWrapper> cacheRoils;

    public IKuroClient WavesClient { get; }
    public ITipShow TipShow { get; }
    public GameRoilDataItem User { get; private set; }

    public GameRoilsViewModel(IKuroClient wavesClient, ITipShow tipShow)
    {
        WavesClient = wavesClient;
        TipShow = tipShow;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        await SetDataAsync(message.Data.Item);
    }

    public async Task SetDataAsync(GameRoilDataItem item)
    {
        await RefreshDataAsync(item);
    }

    private async Task RefreshDataAsync(GameRoilDataItem item)
    {
        SelectFilter = null;
        this.User = item;
        var data = await TryInvokeAsync(async () => await WavesClient.RefreshGamerDataAsync(item, this.CTS.Token));
        if (data.Item1 != 0)
        {
            TipShow.ShowMessage("角色数据请求错误", Symbol.Clear);
            return;
        }
        var GameRoil = await TryInvokeAsync(async () => await WavesClient.GetGamerRoleDataAsync(User, this.CTS.Token));
        if (GameRoil.Item1 != 0)
        {
            TipShow.ShowMessage("数据请求错误", Symbol.Clear);
            return;
        }
        this.cacheRoils = FormatRole(GameRoil.Item2, User);
        if (this.SelectFilter == null)
        {
            this.SelectFilter = GamerFilter[0];
        }
    }

    partial void OnSelectFilterChanged(GamerRoleFilter value)
    {
        if (value == null)
            return;
        if (value.Id == 0)
        {
            this.RoleDatas = this.cacheRoils.ToObservableCollection();
        }
        else
        {
            this.RoleDatas = this
                .cacheRoils.Where(x => x.AttibuteId == value.Id)
                .ToObservableCollection();
        }
    }

    private ObservableCollection<DataCenterRoilItemWrapper> FormatRole(
        GamerRoleData gameRoil,
        GameRoilDataItem user
    )
    {
        ObservableCollection<DataCenterRoilItemWrapper> items = new();
        if (gameRoil == null)
        {
            TipShow.ShowMessage("网络错误！", Symbol.Clear);
            return items;
        }
        foreach (var item in gameRoil.RoleList)
        {
            items.Add(new(item, user));
        }
        return items;
    }

    [ObservableProperty]
    public partial ObservableCollection<DataCenterRoilItemWrapper> RoleDatas { get; set; }

    [ObservableProperty]
    public partial GamerRoleFilter SelectFilter { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<GamerRoleFilter> GamerFilter { get; set; } =
        new ObservableCollection<GamerRoleFilter>()
        {
            new() { DisplayName = "全部", Id = 0 },
            new() { DisplayName = "冷凝", Id = 1 },
            new() { DisplayName = "热熔", Id = 2 },
            new() { DisplayName = "导电", Id = 3 },
            new() { DisplayName = "气动", Id = 4 },
            new() { DisplayName = "衍射", Id = 5 },
            new() { DisplayName = "湮灭", Id = 6 },
        };

    public override void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        RoleDatas.RemoveAll();
        GamerFilter.RemoveAll();
        cacheRoils.RemoveAll();
        this.CTS.Cancel();
    }
}
