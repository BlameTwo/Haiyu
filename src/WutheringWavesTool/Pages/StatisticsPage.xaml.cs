namespace Haiyu.Pages;

public sealed partial class StatisticsPage : Page, IPage
{
    public StatisticsPage()
    {
        InitializeComponent();
    }

    public Type PageType => typeof(StatisticsPage);
}
