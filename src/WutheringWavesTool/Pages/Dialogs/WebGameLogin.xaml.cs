namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class WebGameLogin : ContentDialog,IDialog
{
    public WebGameLogin()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<WebGameViewModel>();
    }

    public WebGameViewModel ViewModel { get; }

    public void SetData(object data)
    {
    }
}
