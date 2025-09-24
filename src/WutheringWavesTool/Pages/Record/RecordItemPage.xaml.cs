namespace Haiyu.Pages.Record;

public sealed partial class RecordItemPage : Page, IPage
{
    public RecordItemPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<RecordItemViewModel>();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is RecordArgs item)
        {
            this.ViewModel.SetData(item);
        }
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
    }

    public Type PageType => typeof(RecordItemPage);

    public RecordItemViewModel ViewModel { get; }
}
