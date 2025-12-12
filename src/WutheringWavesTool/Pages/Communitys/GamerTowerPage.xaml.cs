namespace Haiyu.Pages.Communitys;

public sealed partial class GamerTowerPage : Page, IPage, IDisposable
{
    private bool disposedValue;

    public GamerTowerViewModel ViewModel { get; }

    public Type PageType => typeof(GamerTowerPage);

    public GamerTowerPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Host.Services.GetRequiredService<GamerTowerViewModel>();
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
        Dispose();
        base.OnNavigatedFrom(e);
    }

    public void Dispose()
    {
        this.ViewModel.Dispose();
        GC.Collect();
    }
}
