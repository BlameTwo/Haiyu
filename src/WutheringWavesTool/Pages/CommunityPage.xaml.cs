
namespace Haiyu.Pages;

public sealed partial class CommunityPage : Page, IPage, IDisposable
{
    private bool disposedValue;

    public CommunityPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Host.Services.GetRequiredService<CommunityViewModel>();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        if (this.frame.Content is IDisposable disposable)
        {
            disposable.Dispose();
        }
        this.Dispose();
        GC.Collect();
        base.OnNavigatedFrom(e);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if(e.Parameter is GameRoilDataItem item)
        {
            this.ViewModel.Item = item;
        }
    }

    public Type PageType => typeof(CommunityPage);

    public CommunityViewModel ViewModel { get; private set; }

    private void dataSelect_SelectionChanged(
        SelectorBar sender,
        SelectorBarSelectionChangedEventArgs args
    )
    {
        if (sender.SelectedItem.Tag == null)
            return;
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.ViewModel.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }



    public void SetData(object value)
    {

    }
}
