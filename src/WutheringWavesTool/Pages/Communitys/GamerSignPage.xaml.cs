namespace Haiyu.Pages.Communitys;

public sealed partial class GamerSignPage : Page, IWindowPage
{
    private Window window;

    public GamerSignPage()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service!.GetRequiredService<GamerSignViewModel>();
    }

    public GamerSignViewModel ViewModel { get; }

    public void Dispose() { }

    public void SetData(object value)
    {
        if (value is GameRoilDataItem item)
        {
            this.ViewModel.SignRoil = item;
        }
    }

    public void SetWindow(Window window)
    {
        this.titlebar.Window = window;
        this.titlebar.IsExtendsContentIntoTitleBar = true;
        this.titlebar.UpDate();
    }
}
