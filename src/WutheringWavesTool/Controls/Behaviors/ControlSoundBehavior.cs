namespace Haiyu.Controls.Behaviors
{
    public partial class ControlSoundBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Tapped += AssociatedObject_Tapped;
            base.OnAttached();
        }

        private void AssociatedObject_Tapped(
            object sender,
            Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e
        )
        {
            Sound.PlayClick();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Tapped -= AssociatedObject_Tapped;
            base.OnDetaching();
        }
    }
}
