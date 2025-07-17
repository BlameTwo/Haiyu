namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class GameResourceDialog : ContentDialog
{
    public GameResourceDialog(GameResourceViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
    }

    public GameResourceViewModel ViewModel { get; }

    internal void SetData(string contextName)
    {
        this.ViewModel.SetData(contextName);
    }
}
