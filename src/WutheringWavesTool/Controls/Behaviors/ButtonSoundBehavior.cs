namespace Haiyu.Controls.Behaviors;

public class ButtonSoundBehavior : Behavior<ButtonBase>
{
    protected override void OnAttached()
    {
        AssociatedObject.Click += AssociatedObject_Click;
    }

    private void AssociatedObject_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Sound.PlayClick();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Click -= AssociatedObject_Click;
    }
}
