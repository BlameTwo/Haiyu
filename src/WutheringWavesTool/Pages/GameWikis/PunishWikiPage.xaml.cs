using Haiyu.ViewModel.WikiViewModels;


namespace Haiyu.Pages.GameWikis;

public sealed partial class PunishWikiPage : Page, IPage
{
    public PunishWikiPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<PunishWikiViewModel>();
    }
    public PunishWikiViewModel ViewModel { get; private set; }
    public Type PageType => typeof(PunishWikiPage);


    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.ViewModel.Dispose();
        base.OnNavigatedFrom(e);
    }
}
