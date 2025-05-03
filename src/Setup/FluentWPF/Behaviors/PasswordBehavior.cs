using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace FluentWPF.Behaviors;

public class PasswordBehavior : Behavior<PasswordBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.PasswordChanged += OnPasswordChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.PasswordChanged -= OnPasswordChanged;
    }

    private static void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox box = sender as PasswordBox;
        string password = PasswordAttProperty.GetPassword(box);
        if (box != null && box.Password != password)
        {
            PasswordAttProperty.SetPassword(box, box.Password);
        }
    }
}

public class PasswordAttProperty
{
    public static string GetPassword(DependencyObject obj)
    {
        return (string)obj.GetValue(PasswordProperty);
    }

    public static void SetPassword(DependencyObject obj, string value)
    {
        obj.SetValue(PasswordProperty, value);
    }

    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordAttProperty),
            new PropertyMetadata("", OnPasswordPropertyChanged)
        );

    private static void OnPasswordPropertyChanged(
        DependencyObject sender,
        DependencyPropertyChangedEventArgs e
    )
    {
        PasswordBox box = sender as PasswordBox;
        string password = (string)e.NewValue;
        if (box != null && box.Password != password)
        {
            box.Password = password;
        }
    }
}
