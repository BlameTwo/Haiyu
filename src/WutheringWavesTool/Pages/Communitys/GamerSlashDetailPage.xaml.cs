namespace Haiyu.Pages.Communitys;

public sealed partial class GamerSlashDetailPage : Page, IPage,IDisposable
{
    private bool disposedValue;

    public GamerSlashDetailPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<GamerSlashDetailViewModel>()!;
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

    public Type PageType => typeof(GamerSlashDetailPage);

    public GamerSlashDetailViewModel ViewModel { get; }

}
