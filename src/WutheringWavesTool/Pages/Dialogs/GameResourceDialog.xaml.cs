namespace Haiyu.Pages.Dialogs;

public sealed partial class GameResourceDialog : ContentDialog
{
    public GameResourceDialog(GameResourceViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
    }

    public GameResourceViewModel ViewModel { get; }

    internal void SetData(string contextName)
    {
        this.ViewModel.SetData(contextName);
    }
}
