namespace Haiyu.Pages;

public sealed partial class ToolPage : Page
{
    public ToolPage(ToolViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public ToolViewModel ViewModel { get; }
}
