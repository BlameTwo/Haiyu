namespace Haiyu.Pages;

public sealed partial class TestPage : Page, IPage
{
    public TestPage()
    {
        this.InitializeComponent();
    }

    public Type PageType => typeof(TestPage);
}
