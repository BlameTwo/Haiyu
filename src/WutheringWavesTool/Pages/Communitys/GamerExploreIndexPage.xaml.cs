namespace Haiyu.Pages.Communitys;

public sealed partial class GamerExploreIndexPage : Page, IPage, IDisposable
{
    public GamerExploreIndexPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<GamerExploreIndexViewModel>();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is GameRoilDataItem item)
        {
            await this.ViewModel.SetDataAsync(item);
        }
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.Dispose();
        GC.Collect();
        base.OnNavigatedFrom(e);
    }

    public void Dispose()
    {
        ((IDisposable)ViewModel).Dispose();
    }

    public Type PageType => typeof(GamerExploreIndexPage);

    public GamerExploreIndexViewModel ViewModel { get; private set; }
}
