namespace Haiyu.Pages.Communitys;

public sealed partial class GamerExploreIndexPage : Page, IPage,IDisposable
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
        base.OnNavigatedFrom(e);
    }

    public Type PageType => typeof(GamerExploreIndexPage);

    public GamerExploreIndexViewModel ViewModel { get; private set; }

    public void Dispose()
    {
        this.ViewModel.Dispose();
        GC.Collect();
    }
}
