using System.Numerics;
using CommunityToolkit.WinUI.Animations;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace WutheringWavesTool.Behaviors;

public sealed class PointerMoveScaleBehavior : Behavior<FrameworkElement>
{
    private bool _isAttached;

    protected override void OnAttached()
    {
        if (_isAttached)
            return;

        base.OnAttached();
        AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
        AssociatedObject.PointerExited += AssociatedObject_PointerExited;
        AssociatedObject.PointerCanceled += AssociatedObject_PointerExited;
        _isAttached = true;
    }

    protected override void OnDetaching()
    {
        if (!_isAttached)
            return;

        AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
        AssociatedObject.PointerExited -= AssociatedObject_PointerExited;
        AssociatedObject.PointerCanceled -= AssociatedObject_PointerExited;
        base.OnDetaching();
        _isAttached = false;
    }

    private void AssociatedObject_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        
    }

    private void AssociatedObject_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        
    }
}