using System.Windows.Controls;

namespace FluentWPF.Controls;


[TemplatePart(Name = "ColorBorder",Type =typeof(Border))]
[TemplatePart(Name = "DefaultBorder", Type =typeof(Border))]
[TemplatePart(Name = "PARA_Content", Type =typeof(ContentPresenter))]
public class FluentToggleButton : ToggleButton
{
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(FluentToggleButton),
        new PropertyMetadata(null)
    );
}
