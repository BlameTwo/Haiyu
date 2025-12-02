namespace Haiyu.Pages.Communitys;

public sealed partial class GamerDockPage : Page, IPage,IDisposable
{
    private bool disposedValue;

    public GamerDockPage()
    {
        this.InitializeComponent();

        this.ViewModel = Instance.Service.GetRequiredService<GamerDockViewModel>();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is GameRoilDataItem item)
        {
            await this.ViewModel.SetDataAsync(item);
        }
        base.OnNavigatedTo(e);
    }


    public GamerDockViewModel ViewModel { get; }

    public Type PageType => typeof(GamerDockViewModel);


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
