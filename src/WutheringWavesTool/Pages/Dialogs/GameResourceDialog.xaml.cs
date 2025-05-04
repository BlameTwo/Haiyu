namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class GameResourceDialog : ContentDialog
{
    public GameResourceDialog(GameResourceViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
        this.RequestedTheme =
            AppSettings.AppTheme == null ? ElementTheme.Default
            : AppSettings.AppTheme == "Dark" ? ElementTheme.Dark
            : AppSettings.AppTheme == "Light" ? ElementTheme.Light
            : ElementTheme.Default;
    }

    public GameResourceViewModel ViewModel { get; }

    internal void SetData(string contextName)
    {
        this.ViewModel.SetData(contextName);
    }
}
