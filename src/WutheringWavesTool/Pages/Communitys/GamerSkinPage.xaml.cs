namespace Haiyu.Pages.Communitys;

public sealed partial class GamerSkinPage : Page, IPage,IDisposable
{
    public GamerSkinPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<GamerSkinViewModel>();
    }

    public GamerSkinViewModel ViewModel { get; }

    public Type PageType => typeof(GamerSkinPage);

    public void Dispose()
    {
        this.ViewModel.Dispose();
        GC.Collect();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        Dispose();
        base.OnNavigatedFrom(e);
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is GameRoilDataItem item)
        {
            this.ViewModel.SetData(item);
        }
        base.OnNavigatedTo(e);
    }

}
