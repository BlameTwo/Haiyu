namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class LoginDialog : ContentDialog, IDialog
{
    public LoginDialog()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<LoginViewModel>();
        this.RequestedTheme = this.RequestedTheme = ElementTheme.Dark;
    }

    public LoginViewModel ViewModel { get; }

    public void SetData(object data) { }

    private void SelectorBar_SelectionChanged(
        SelectorBar sender,
        SelectorBarSelectionChangedEventArgs args
    )
    {
        this.ViewModel.SwitchView(sender.SelectedItem.Tag.ToString());
    }
}
