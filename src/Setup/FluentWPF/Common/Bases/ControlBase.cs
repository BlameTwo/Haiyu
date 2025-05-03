using System.Windows.Controls;

namespace FluentWPF.Common.Bases;

/// <summary>
/// 控件基础
/// </summary>
public abstract class ControlBase : Control
{
    //这里有一个控件基础，用来写自定义控件



    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(ControlBase),
        new PropertyMetadata(null)
    );
}
