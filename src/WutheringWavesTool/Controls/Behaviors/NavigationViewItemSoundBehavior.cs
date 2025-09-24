using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiyu.Controls.Behaviors
{
    internal class NavigationViewItemSoundBehavior : Behavior<NavigationViewItem>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed;
            base.OnDetaching();
        }

        private void AssociatedObject_PointerPressed(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e
        )
        {
            Sound.PlayClick();
        }
    }
}
