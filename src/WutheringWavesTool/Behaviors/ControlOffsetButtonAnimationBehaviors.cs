using System.Numerics;
using CommunityToolkit.WinUI.Animations;

namespace Haiyu.Behaviors;

public sealed class ControlOffsetButtonAnimationBehaviors : Behavior<Button>
{
    protected override void OnAttached()
    {
        this.AssociatedObject.Click += AssociatedObject_Click;
        this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        base.OnAttached();
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.IsOpen)
        {
            this.AssociatedObject.Content = CloseIcon;
        }
        else
        {
            this.AssociatedObject.Content = OpenIcon;
        }
        this.OpenOffset = this.Owner.ActualOffset;
    }

    protected override void OnDetaching()
    {
        this.AssociatedObject.Click -= AssociatedObject_Click;
        base.OnDetaching();
    }

    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        "IsOpen",
        typeof(bool),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(true, OnIsOpenCallBack)
    );

    public Vector3? OpenOffset { get; set; }

    private static void OnIsOpenCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ControlOffsetButtonAnimationBehaviors b)
        {
            b.Update();
        }
    }

    private void Update()
    {
        if (Owner == null || OpenOffset == null)
            return;
        if (this.IsOpen)
        {
            AnimationSet sets = new();
            sets.Add(
                new OffsetAnimation()
                {
                    To = $"{OpenOffset.Value.X},{OpenOffset.Value.Y},{OpenOffset.Value.Z}",
                    EasingMode = EasingMode.EaseInOut,
                    EasingType = EasingType.Cubic,
                    Duration = this.Duration,
                }
            );
            this.AssociatedObject.Content = CloseIcon;
            sets.Start(this.Owner);
        }
        else
        {
            if (Orientation == Orientation.Horizontal)
            {
                AnimationSet sets = new();
                sets.Add(
                    new OffsetAnimation()
                    {
                        To =
                            $"{Owner.ActualOffset.X - Owner.ActualWidth + this.AssociatedObject.ActualWidth - Offset},{Owner.ActualOffset.Y},{Owner.ActualOffset.Z}",
                        EasingMode = EasingMode.EaseInOut,
                        EasingType = EasingType.Cubic,
                        Duration = this.Duration,
                    }
                );
                sets.Start(this.Owner);
            }
            else
            {
                AnimationSet sets = new();
                sets.Add(
                    new OffsetAnimation()
                    {
                        To =
                            $"{Owner.ActualOffset.X},{Owner.ActualOffset.Y - Owner.ActualHeight + -this.AssociatedObject.ActualWidth - Offset},{Owner.ActualOffset.Z}",
                        EasingMode = EasingMode.EaseInOut,
                        EasingType = EasingType.Cubic,
                        Duration = this.Duration,
                    }
                );
                sets.Start(this.Owner);
            }

            this.AssociatedObject.Content = OpenIcon;
        }
    }

    public FrameworkElement Owner
    {
        get { return (FrameworkElement)GetValue(OwnerProperty); }
        set { SetValue(OwnerProperty, value); }
    }

    public double Offset
    {
        get { return (double)GetValue(OffsetProperty); }
        set { SetValue(OffsetProperty, value); }
    }

    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
        "Offset",
        typeof(double),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(
            0.0,
            (s, e) =>
            {
                if (s is ControlOffsetButtonAnimationBehaviors b)
                {
                    b.Update();
                }
            }
        )
    );

    public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(
        "Owner",
        typeof(FrameworkElement),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(null)
    );

    public TimeSpan Duration
    {
        get { return (TimeSpan)GetValue(DurationProperty); }
        set { SetValue(DurationProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        "Duration",
        typeof(TimeSpan),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(TimeSpan.FromSeconds(0.3))
    );

    public Orientation Orientation
    {
        get { return (Orientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
    }

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        "Orientation",
        typeof(Orientation),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(Orientation.Horizontal)
    );

    private void AssociatedObject_Click(object sender, RoutedEventArgs e)
    {
        this.IsOpen = !IsOpen;
    }

    public FontIcon OpenIcon
    {
        get { return (FontIcon)GetValue(OpenIconProperty); }
        set { SetValue(OpenIconProperty, value); }
    }

    // Using a DependencyProperty as the backing store for OpenIcon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OpenIconProperty = DependencyProperty.Register(
        "OpenIcon",
        typeof(FontIcon),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(null)
    );

    public FontIcon CloseIcon
    {
        get { return (FontIcon)GetValue(CloseIconProperty); }
        set { SetValue(CloseIconProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CloseIcon.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CloseIconProperty = DependencyProperty.Register(
        "CloseIcon",
        typeof(FontIcon),
        typeof(ControlOffsetButtonAnimationBehaviors),
        new PropertyMetadata(null)
    );
}
