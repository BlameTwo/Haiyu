using System.Windows.Controls;

namespace FluentWPF.Controls;

public class FluentComboBox : ComboBox
{
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(FluentComboBox),
        new PropertyMetadata(null)
    );



}
