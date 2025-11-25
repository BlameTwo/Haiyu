namespace Haiyu.Pages.Communitys;

public sealed partial class GamerChallengePage : Page, IPage
{
    private bool disposedValue;

    public GamerChallengeViewModel ViewModel { get; }

    public Type PageType => typeof(GamerChallengePage);

    public GamerChallengePage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<GamerChallengeViewModel>();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is GameRoilDataItem item)
        {
            this.ViewModel.SetData(item);
        }
        base.OnNavigatedTo(e);
    }



    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.ViewModel.Dispose();
        base.OnNavigatedFrom(e);
        GC.Collect();
    }
}
