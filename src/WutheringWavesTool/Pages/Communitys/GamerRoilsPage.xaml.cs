namespace Haiyu.Pages.Communitys;

public sealed partial class GamerRoilsPage : Page, IPage
{
    private GameRoilsViewModel viewModel;
    private bool disposedValue;

    public GamerRoilsPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<GameRoilsViewModel>();
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
        this.ViewModel.Dispose();
        base.OnNavigatedFrom(e);
        GC.Collect();
    }

    public Type PageType => typeof(GamerRoilsPage);

    public GameRoilsViewModel ViewModel
    {
        get => viewModel;
        set => viewModel = value;
    }

   
}
