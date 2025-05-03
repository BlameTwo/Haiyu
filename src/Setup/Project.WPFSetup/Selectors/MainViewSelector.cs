using System.Windows;
using System.Windows.Controls;
using Project.WPFSetup.ViewModels.UserViewModels;

namespace Project.WPFSetup.Selectors;

public partial class MainViewSelector : DataTemplateSelector
{
    public DataTemplate InstallTemplate { get; set; }

    public DataTemplate UninstallTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is InstallViewModel)
        {
            return InstallTemplate;
        }
        if (item is UninstallViewModel)
        {
            return UninstallTemplate;
        }
        return base.SelectTemplate(item, container);
    }
}
