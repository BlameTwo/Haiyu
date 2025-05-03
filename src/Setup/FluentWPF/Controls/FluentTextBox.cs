using System.Windows.Controls;

namespace FluentWPF.Controls;

public class FluentTextBox : TextBox
{
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(FluentTextBox),
        new PropertyMetadata(default(CornerRadius))
    );



    public Visibility ClearButtonVisibility
    {
        get { return (Visibility)GetValue(ClearButtonVisibilityProperty); }
        set { SetValue(ClearButtonVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ClearButtonVisibility.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ClearButtonVisibilityProperty =
        DependencyProperty.Register("ClearButtonVisibility", typeof(Visibility), typeof(FluentTextBox), new PropertyMetadata(Visibility.Visible));


}
