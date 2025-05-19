using WutheringWavesTool.Models.Wrapper.CommunitySlash;

namespace WutheringWavesTool.ViewModel.Communitys;

public sealed partial class GamerSlashDetailViewModel : ViewModelBase, IDisposable
{
    private bool disposedValue;

    public GamerSlashDetailViewModel(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        await this.RefreshData();
    }

    public IWavesClient WavesClient { get; }
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
        var result = await WavesClient.GetGamerSlashDetailAsync(this.Roil, this.CTS.Token);
        if (result == null || result.DifficultyList == null)
            return;
        var diff0 = result.DifficultyList.Where(x => x.Difficulty == 0).FirstOrDefault();
        var diff1 = result.DifficultyList.Where(x => x.Difficulty == 1).FirstOrDefault();
        var diff2 = result.DifficultyList.Where(x => x.Difficulty == 2).FirstOrDefault();
        this.Difficulty0Header = SlashHeaderWrapper.Convert(diff0);
        this.Difficulty1Header = SlashHeaderWrapper.Convert(diff1);
        this.Difficulty2Header = SlashHeaderWrapper.Convert(diff2);
        this.SlashHeader0Items = SlashItemWrapper.Convert(diff0.ChallengeList);
        this.SlashHeader1Items = SlashItemWrapper.Convert(diff1.ChallengeList);
        var SlashHeader2Items = SlashItemWrapper.Convert(diff2.ChallengeList);
        SlashHeader1Items.Insert(0, SlashHeader2Items[0]);
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.CTS.Cancel();
                this.Difficulty0Header = null;
                this.Difficulty1Header = null;
                this.Difficulty2Header = null;
                this.SlashHeader0Items = null;
                this.SlashHeader1Items = null;
                WeakReferenceMessenger.Default.UnregisterAll(this);
            }
            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
