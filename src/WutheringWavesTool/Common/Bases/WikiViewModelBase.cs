namespace Haiyu.Common.Bases;

public partial class WikiViewModelBase : ViewModelBase
{
    public WikiViewModelBase()
    {
        GameWikiClient = Instance.GetService<IGameWikiClient>();
        WavesClient = Instance.GetService<IWavesClient>();
        this.TipShow = Instance.GetService<ITipShow>();
    }

    public IGameWikiClient GameWikiClient { get; }

    public IWavesClient WavesClient { get;  }

    public ITipShow TipShow { get; }

    [ObservableProperty]
    public partial bool IsLogin { get; set; }
}
