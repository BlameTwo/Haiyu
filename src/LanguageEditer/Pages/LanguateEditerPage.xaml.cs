using LanguageEditer.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace LanguageEditer.Pages;

public sealed partial class LanguateEditerPage : Page
{
    public LanguateEditerPage()
    {
        InitializeComponent();
        this.ViewModel = new LanguageEditerViewModel();
        this.ViewModel.SetDataGrid(this.dataGrid);
    }

    internal LanguageEditerViewModel ViewModel { get; }

}
