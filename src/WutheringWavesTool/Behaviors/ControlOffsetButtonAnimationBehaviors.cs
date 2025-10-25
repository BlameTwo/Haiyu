using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Numerics;
using CommunityToolkit.WinUI.Animations;

namespace Haiyu.Behaviors;

public sealed class ControlOffsetButtonAnimationBehaviors : Behavior<Button>
{
    private Vector3 _initialOffset; // 记录初始位置

    protected override void OnAttached()
    {
        base.OnAttached();
        this.AssociatedObject.Click += AssociatedObject_Click;
        this.AssociatedObject.Loaded += AssociatedObject_Loaded;
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        // 记录初始位置
        _initialOffset = Owner?.ActualOffset ?? default;

        // 初始化按钮图标
        if (IsOpen)
        {
            AssociatedObject.Content = CloseIcon;
        }
        else
        {
            AssociatedObject.Content = OpenIcon;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        this.AssociatedObject.Click -= AssociatedObject_Click;
        this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
    }

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        nameof(IsOpen),
        typeof(bool),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(true, (s, e) =>
        {
            if (s is ControlOffsetButtonAnimationBehaviors b)
            {
                b.AnimateToggle();
            }
        })
    );

    public FrameworkElement Owner
    {
        get => (FrameworkElement)GetValue(OwnerProperty);
        set => SetValue(OwnerProperty, value);
    }

    public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
        nameof(Owner),
        typeof(FrameworkElement),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(null)
    );

    public double Offset
    {
        get => (double)GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        nameof(Offset),
        typeof(double),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(0.0)
    );

    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(Orientation.Horizontal)
    );

    public TimeSpan Duration
    {
        get => (TimeSpan)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        nameof(Duration),
        typeof(TimeSpan),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(TimeSpan.FromSeconds(0.3))
    );

    public FontIcon OpenIcon
    {
        get => (FontIcon)GetValue(OpenIconProperty);
        set => SetValue(OpenIconProperty, value);
    }

    public static readonly DependencyProperty OpenIconProperty = DependencyProperty.Register(
        nameof(OpenIcon),
        typeof(FontIcon),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(null)
    );

    public FontIcon CloseIcon
    {
        get => (FontIcon)GetValue(CloseIconProperty);
        set => SetValue(CloseIconProperty, value);
    }

    public static readonly DependencyProperty CloseIconProperty = DependencyProperty.Register(
        nameof(CloseIcon),
        typeof(FontIcon),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(null)
    );

    private void AssociatedObject_Click(object sender, RoutedEventArgs e)
    {
        IsOpen = !IsOpen;
    }

    private void AnimateToggle()
    {
        if (Owner == null)
        {
            return;
        }

        AnimationSet animationSet = new();
        var easingConfig = new AnimationSet();

        if (IsOpen)
        {
            // 打开：还原到初始位置
            animationSet.Add(
                new OffsetAnimation
                {
                    To = $"{_initialOffset.X},{_initialOffset.Y},{_initialOffset.Z}",
                    Duration = Duration,
                    EasingType = EasingType.Cubic
                }
            );
            AssociatedObject.Content = CloseIcon;
        }
        else
        {
            // 关闭：动态计算偏移位置
            Vector3 targetOffset;
            if (Orientation == Orientation.Horizontal)
            {
                // 水平方向：向左偏移（收起）
                double offsetX = Owner.ActualOffset.X - Owner.ActualWidth + AssociatedObject.ActualWidth - Offset;
                targetOffset = new Vector3((float)offsetX, (float)_initialOffset.Y, (float)_initialOffset.Z);
            }
            else
            {
                // 垂直方向：向上偏移（收起）
                double offsetY = Owner.ActualOffset.Y - Owner.ActualHeight + AssociatedObject.ActualHeight - Offset;
                targetOffset = new Vector3((float)_initialOffset.X, (float)offsetY, (float)_initialOffset.Z);
            }

            animationSet.Add(
                new OffsetAnimation
                {
                    To = $"{targetOffset.X},{targetOffset.Y},{targetOffset.Z}",
                    Duration = Duration,
                    EasingType = EasingType.Cubic
                }
            );
            AssociatedObject.Content = OpenIcon;
        }

        animationSet.Start(Owner);
    }
}