namespace Haiyu.Pages;

public sealed partial class SettingPage : Page, IPage
{
    public SettingPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<SettingViewModel>();
    }

    public Type PageType => typeof(SettingPage);

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        GC.Collect();
        this.ViewModel.Dispose();
        this.Bindings.StopTracking();
        base.OnNavigatedFrom(e);
    }

    public SettingViewModel ViewModel { get; }
}
