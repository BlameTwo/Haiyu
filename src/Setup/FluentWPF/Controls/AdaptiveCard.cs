using FluentWPF.Common.Bases;

namespace FluentWPF.Controls;

public class AdaptiveCard:ButtonBase
{


    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(AdaptiveCard), new PropertyMetadata(new CornerRadius(0)));


}