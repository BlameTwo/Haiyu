namespace WutheringWavesTool.Pages;

public sealed partial class ActivityTimePage : Page,IWindowPage
{
    public ActivityTimePage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<ActivityTimeViewModel>();
    }

    public ActivityTimeViewModel ViewModel { get; }

    public void Dispose()
    {
    }

    public void SetData(object value)
    {
    }

    public void SetWindow(Window window)
    {
        this.title.Window = window;
        this.title.UpDate();
    }
}
