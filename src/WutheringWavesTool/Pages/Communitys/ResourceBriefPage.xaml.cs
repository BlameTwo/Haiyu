namespace Haiyu.Pages.Communitys;

public sealed partial class ResourceBriefPage : Page, IPage
{
    public ResourceBriefPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<ResourceBriefViewModel>();
    }

    public Type PageType => typeof(ResourceBriefPage);

    public ResourceBriefViewModel ViewModel { get; }

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
        this.ViewModel.Dispose();
        base.OnNavigatedFrom(e);
        GC.Collect();
    }

    public void Dispose()
    {
        ViewModel.Dispose();
    }

}
