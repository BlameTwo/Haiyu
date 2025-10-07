namespace Haiyu.ViewModel;

public sealed partial class StartColorFullGameViewModel:ViewModelBase
{
    public StartColorFullGameViewModel([FromKeyedServices("Cache")]ITipShow tipShow)
    {
        TipShow = tipShow;
    }

    public ITipShow TipShow { get; }
}
