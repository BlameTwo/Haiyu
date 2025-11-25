using Haiyu.Models.Wrapper.CommunitySlash;

namespace Haiyu.ViewModel.Communitys;

public sealed partial class GamerSlashDetailViewModel : ViewModelBase, IDisposable
{
    private bool disposedValue;

    public GamerSlashDetailViewModel(IWavesClient wavesClient, ITipShow tipShow)
    {
        WavesClient = wavesClient;
        TipShow = tipShow;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        await this.RefreshData();
    }

    public IWavesClient WavesClient { get; }
    public ITipShow TipShow { get; }
    public GameRoilDataItem Roil { get; private set; }

    #region Data
    #region Header

    [ObservableProperty]
    public partial SlashHeaderWrapper? Difficulty0Header { get; set; }

    [ObservableProperty]
    public partial SlashHeaderWrapper? Difficulty1Header { get; set; }

    [ObservableProperty]
    public partial SlashHeaderWrapper? Difficulty2Header { get; set; }
    #endregion
    #region HeaderItem
    [ObservableProperty]
    public partial ObservableCollection<SlashItemWrapper>? SlashHeader0Items { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<SlashItemWrapper>? SlashHeader1Items { get; set; }

    #endregion
    #endregion

    public void SetData(GameRoilDataItem item)
    {
        this.Roil = item;
    }

    [RelayCommand]
    async Task Loaded()
    {
        await RefreshData();
    }

    async Task RefreshData()
    {
        var result = await TryInvokeAsync(async () => await WavesClient.GetGamerSlashDetailAsync(this.Roil, this.CTS.Token));
        if (result.Item1 != 0 || result.Item2.DifficultyList == null)
        {
            TipShow.ShowMessage("角色数据请求错误", Symbol.Clear);
            return;
        }
        var diff0 = result.Item2.DifficultyList.Where(x => x.Difficulty == 0).FirstOrDefault();
        var diff1 = result.Item2.DifficultyList.Where(x => x.Difficulty == 1).FirstOrDefault();
        var diff2 = result.Item2.DifficultyList.Where(x => x.Difficulty == 2).FirstOrDefault();
        this.Difficulty0Header = SlashHeaderWrapper.Convert(diff0);
        this.SlashHeader0Items = SlashItemWrapper.Convert(diff0.ChallengeList);
    }

    public override void Dispose()
    {
        this.CTS.Cancel();
        this.Difficulty0Header = null;
        this.Difficulty1Header = null;
        this.Difficulty2Header = null;
        this.SlashHeader0Items = null;
        this.SlashHeader1Items = null;
        WeakReferenceMessenger.Default.UnregisterAll(this);
        GC.SuppressFinalize(this);
    }
}
