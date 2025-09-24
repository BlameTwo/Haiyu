namespace Haiyu.Pages.Communitys;

public sealed partial class GamerSkinPage : Page, IPage
{
    public GamerSkinPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<GamerSkinViewModel>();
    }

    public GamerSkinViewModel ViewModel { get; }

    public Type PageType => typeof(GamerSkinPage);

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is GameRoilDataItem item)
        {
            this.ViewModel.SetData(item);
        }
        base.OnNavigatedTo(e);
    }
}
