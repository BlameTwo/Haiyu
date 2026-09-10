namespace Haiyu.Controls.Propertys;

public class NavigationItemClickHelper
{
    public static string GetNavigationKey(DependencyObject obj)
    {
        return (string)obj.GetValue(NavigationKeyProperty);
    }

    public static void SetNavigationKey(DependencyObject obj, string value)
    {
        obj.SetValue(NavigationKeyProperty, value);
    }

    public static readonly DependencyProperty NavigationKeyProperty =
        DependencyProperty.RegisterAttached(
            "NavigationKey",
            typeof(string),
            typeof(NavigationItemClickHelper),
            new PropertyMetadata("")
        );

    public static string GetParameter(DependencyObject obj)
    {
        return (string)obj.GetValue(ParameterProperty);
    }

    public static void SetParameter(DependencyObject obj, string value)
    {
        obj.SetValue(ParameterProperty, value);
    }

    // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ParameterProperty =
        DependencyProperty.RegisterAttached(
            "Parameter",
            typeof(string),
            typeof(NavigationItemClickHelper),
            new PropertyMetadata("")
        );
}
