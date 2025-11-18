namespace Haiyu.Pages;

public sealed partial class CloudGamePage : Page, IPage
{
    public CloudGamePage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<CloudGameViewModel>();
    }

    public Type PageType => typeof(CloudGamePage);

    public CloudGameViewModel ViewModel { get; }
}
