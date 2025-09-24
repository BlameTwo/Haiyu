namespace Haiyu.Pages.Communitys;

public sealed partial class GamerSlashDetailPage : Page, IPage, IDisposable
{
    private bool disposedValue;

    public GamerSlashDetailPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<GamerSlashDetailViewModel>()!;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is GameRoilDataItem item)
        {
            this.ViewModel.SetData(item);
        }
        base.OnNavigatedTo(e);
    }

    public Type PageType => typeof(GamerSlashDetailPage);

    public GamerSlashDetailViewModel ViewModel { get; }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.Bindings.StopTracking();
                this.ViewModel.Dispose();
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~GamerSlashDetailPage()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
