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

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        (Instance.Service.GetRequiredService<IAppContext<App>>().App.MainWindow.Content as Page)?.RequestedTheme = ElementTheme.Light;
    }

    private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
    {

        (Instance.Service.GetRequiredService<IAppContext<App>>().App.MainWindow.Content as Page)?.RequestedTheme = ElementTheme.Dark;
    }

    private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
    {

        (Instance.Service.GetRequiredService<IAppContext<App>>().App.MainWindow.Content as Page)?.RequestedTheme = ElementTheme.Default;
    }
}
