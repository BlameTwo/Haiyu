namespace Haiyu.Pages.Communitys;

public sealed partial class GamerChallengePage : Page, IPage,IDisposable
{
    private bool disposedValue;

    public GamerChallengeViewModel ViewModel { get; }

    public Type PageType => typeof(GamerChallengePage);

    public GamerChallengePage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Host.Services.GetRequiredService<GamerChallengeViewModel>();
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
        this.Dispose();
        base.OnNavigatedFrom(e);
    }

    public void Dispose()
    {
        this.ViewModel.Dispose();
        GC.Collect();
    }
}
