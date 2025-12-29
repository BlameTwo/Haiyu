using Haiyu.Models.Wrapper.Wiki;

namespace Haiyu.Controls.Selector;

public partial class WavesPlaycardSelector : DataTemplateSelector
{
    public DataTemplate? SmallImageTempalte { get; set; }
    public DataTemplate? BigImageTemplate { get; set; }
    protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is EventContentSideWrapper sideWrapper)
        {
            if (sideWrapper.ImgMode == "4-h")
            {
                return SmallImageTempalte;
            }
            else
            {
                return BigImageTemplate;
            }
        }
        return base.SelectTemplateCore(item, container);
    }
}
