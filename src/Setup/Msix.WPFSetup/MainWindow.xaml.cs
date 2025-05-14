using Msix.WPFSetup.ViewModels;
using Wpf.Ui.Controls;

namespace Msix.WPFSetup;

public partial class MainWindow : FluentWindow
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowViewModel();
    }
}
