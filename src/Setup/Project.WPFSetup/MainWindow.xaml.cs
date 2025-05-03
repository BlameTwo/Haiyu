using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluentWPF.Controls;
using FluentWPF.Controls.SystemBackdrops;
using Project.WPFSetup.ViewModels;

namespace Project.WPFSetup
{
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.GetService<MainWindowViewModel>();
        }
    }
}
