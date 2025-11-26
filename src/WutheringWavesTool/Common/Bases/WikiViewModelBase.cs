namespace Haiyu.Common.Bases;

public partial class WikiViewModelBase : ViewModelBase
{
    public WikiViewModelBase()
    {
        GameWikiClient = Instance.Service.GetRequiredService<IGameWikiClient>();
        WavesClient = Instance.Service.GetRequiredService<IKuroClient>();
        this.TipShow = Instance.Service.GetRequiredService<ITipShow>();
    }

    public IGameWikiClient GameWikiClient { get; }

    public IKuroClient WavesClient { get;  }

    public ITipShow TipShow { get; }

    [ObservableProperty]
    public partial bool IsLogin { get; set; }
}
