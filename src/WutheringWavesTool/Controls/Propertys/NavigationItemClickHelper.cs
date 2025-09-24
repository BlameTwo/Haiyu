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

    public static string GetParamter(DependencyObject obj)
    {
        return (string)obj.GetValue(ParamterProperty);
    }

    public static void SetParamter(DependencyObject obj, string value)
    {
        obj.SetValue(ParamterProperty, value);
    }

    // Using a DependencyProperty as the backing store for Paramter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ParamterProperty =
        DependencyProperty.RegisterAttached(
            "Paramter",
            typeof(string),
            typeof(NavigationItemClickHelper),
            new PropertyMetadata("")
        );
}
