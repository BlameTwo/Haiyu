namespace Haiyu.Controls.Selector;

public partial class CommunitySelector : DataTemplateSelector
{
    public DataTemplate GamerRoilsPage { get; set; }

    public DataTemplate GamerDockPage { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        return base.SelectTemplateCore(item);
    }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is GameRoilsViewModel)
        {
            return GamerRoilsPage;
        }
        if (item is GamerDockViewModel)
            return GamerDockPage;
        return base.SelectTemplateCore(item, container);
    }
}
