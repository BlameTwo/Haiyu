namespace FluentWPF.Controls;

public class FluentRepeatButton : RepeatButton
{
    public FluentRepeatButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(FluentRepeatButton),
            new FrameworkPropertyMetadata(typeof(FluentRepeatButton))
        );
    }

    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(FluentRepeatButton),
        new PropertyMetadata(default(CornerRadius))
    );
}
