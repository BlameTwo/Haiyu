namespace WutheringWavesTool.Pages.Communitys;

public sealed partial class ResourceBriefPage : Page,IPage
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
        this.Dispose();
        GC.Collect();
        base.OnNavigatedFrom(e);
    }

    public void Dispose()
    {
        ViewModel.Dispose();
    }
}
