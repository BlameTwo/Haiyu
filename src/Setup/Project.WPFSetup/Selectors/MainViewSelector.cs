using System.Windows;
using System.Windows.Controls;
using Project.WPFSetup.ViewModels.UserViewModels;

namespace Project.WPFSetup.Selectors;

public partial class MainViewSelector : DataTemplateSelector
{
    public DataTemplate? InstallTemplate { get; set; }

    public DataTemplate? UninstallTemplate { get; set; }

    public DataTemplate? UpdateTemplate { get; set; }

    public DataTemplate? RepairTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is InstallViewModel)
        {
            return InstallTemplate;
        }
        if (item is UninstallViewModel)
        {
            return UninstallTemplate;
        }
        if (item is UpdateViewModel)
        {
            return UpdateTemplate;
        }
        if (item is RepairViewModel)
        {
            return RepairTemplate;
        }
        return base.SelectTemplate(item, container);
    }
}
