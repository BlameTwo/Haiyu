namespace Haiyu.Behaviors;

public sealed class FilpViewAutoPlayBehavior : Behavior<FlipView>
{
    public DispatcherTimer? Timer { get; set; }
    public bool IsPlay
    {
        get { return (bool)GetValue(IsPlayProperty); }
        set { SetValue(IsPlayProperty, value); }
    }

    public static readonly DependencyProperty IsPlayProperty = DependencyProperty.Register(
        "IsPlay",
        typeof(bool),
        typeof(FilpViewAutoPlayBehavior),
        new PropertyMetadata(false, IsPlayCallback)
    );

    public TimeSpan Duration
    {
        get { return (TimeSpan)GetValue(DurationProperty); }
        set { SetValue(DurationProperty, value); }
    }

    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        "Duration",
        typeof(TimeSpan),
        typeof(TimeSpan),
        new PropertyMetadata(TimeSpan.FromSeconds(3))
    );

    private static void IsPlayCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FilpViewAutoPlayBehavior b)
        {
            b.Update();
        }
    }

    private void Update()
    {
        if (Timer != null)
        {
            Timer.Tick -= Timer_Tick;
            Timer.Stop();
            Timer = null;
        }
        Timer = new DispatcherTimer();
        Timer.Interval = Duration;
        Timer.Tick += Timer_Tick;
        Timer.Start();
    }

    protected override void OnDetaching()
    {
        if (Timer != null)
        {
            Timer.Stop();
            Timer.Tick -= Timer_Tick;
        }
        base.OnDetaching();
    }

    private void Timer_Tick(object? sender, object e)
    {
        var max = this.AssociatedObject.Items.Count - 1;
        var index = this.AssociatedObject.SelectedIndex;
        if (index == max)
        {
            this.AssociatedObject.SelectedIndex = 0;
        }
        else
        {
            this.AssociatedObject.SelectedIndex += 1;
        }
    }
}
