using Haiyu.Models.Wrapper.WindowRoils;

namespace Haiyu.Controls.Selector;

public partial class GamerRoilDetilySelector : DataTemplateSelector
{
    public DataTemplate TypeItemTemplate { get; set; }

    public DataTemplate RoleItemTempalte { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is NavigationRoilsDetilyItem)
        {
            return RoleItemTempalte;
        }
        else
        {
            return TypeItemTemplate;
        }
    }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is NavigationRoilsDetilyItem)
        {
            return RoleItemTempalte;
        }
        else
        {
            return TypeItemTemplate;
        }
    }
}
