using Windows.UI.ViewManagement;

namespace Haiyu.Controls.Propertys;

public static class LauncherButtonShine
{
    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(LauncherButtonShine),
            new PropertyMetadata(false, OnIsEnabledChanged)
        );

    public static readonly DependencyProperty PlayOnPointerOverProperty =
        DependencyProperty.RegisterAttached(
            "PlayOnPointerOver",
            typeof(bool),
            typeof(LauncherButtonShine),
            new PropertyMetadata(true)
        );

    public static readonly DependencyProperty PlayWhenEnabledProperty =
        DependencyProperty.RegisterAttached(
            "PlayWhenEnabled",
            typeof(bool),
            typeof(LauncherButtonShine),
            new PropertyMetadata(true)
        );

    public static readonly DependencyProperty PulseProperty = DependencyProperty.RegisterAttached(
        "Pulse",
        typeof(int),
        typeof(LauncherButtonShine),
        new PropertyMetadata(0, OnPulseChanged)
    );

    private static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached(
        "State",
        typeof(ShineState),
        typeof(LauncherButtonShine),
        new PropertyMetadata(null)
    );

    public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

    public static void SetIsEnabled(DependencyObject obj, bool value) =>
        obj.SetValue(IsEnabledProperty, value);

    public static bool GetPlayOnPointerOver(DependencyObject obj) =>
        (bool)obj.GetValue(PlayOnPointerOverProperty);

    public static void SetPlayOnPointerOver(DependencyObject obj, bool value) =>
        obj.SetValue(PlayOnPointerOverProperty, value);

    public static bool GetPlayWhenEnabled(DependencyObject obj) =>
        (bool)obj.GetValue(PlayWhenEnabledProperty);

    public static void SetPlayWhenEnabled(DependencyObject obj, bool value) =>
        obj.SetValue(PlayWhenEnabledProperty, value);

    public static int GetPulse(DependencyObject obj) => (int)obj.GetValue(PulseProperty);

    public static void SetPulse(DependencyObject obj, int value) =>
        obj.SetValue(PulseProperty, value);

    public static void Play(Button button)
    {
        if (button.GetValue(StateProperty) is ShineState state)
        {
            state.Play();
        }
    }

    private static void OnIsEnabledChanged(
        DependencyObject sender,
        DependencyPropertyChangedEventArgs args
    )
    {
        if (sender is not Button button)
            return;
        if (button.GetValue(StateProperty) is ShineState oldState)
        {
            oldState.Dispose();
            button.ClearValue(StateProperty);
        }
        if ((bool)args.NewValue)
        {
            button.SetValue(StateProperty, new ShineState(button));
        }
    }

    private static void OnPulseChanged(
        DependencyObject sender,
        DependencyPropertyChangedEventArgs args
    )
    {
        if (sender is Button button && GetIsEnabled(button))
            Play(button);
    }

    private sealed class ShineState : IDisposable
    {
        private readonly Button button;
        private readonly UISettings uiSettings = new();
        private ShapeVisual? visual;
        private CompositionRoundedRectangleGeometry? geometry;
        private CompositionLinearGradientBrush? brush;
        private CompositionScopedBatch? batch;
        private bool pendingPlay;

        public ShineState(Button button)
        {
            this.button = button;
            button.Loaded += OnLoaded;
            button.Unloaded += OnUnloaded;
            button.SizeChanged += OnSizeChanged;
            button.PointerEntered += OnPointerEntered;
            button.IsEnabledChanged += OnButtonIsEnabledChanged;
            button.ActualThemeChanged += OnActualThemeChanged;
            if (button.IsLoaded)
                Initialize();
        }

        public void Play()
        {
            if (!button.IsEnabled || !uiSettings.AnimationsEnabled)
                return;
            if (!button.IsLoaded)
            {
                pendingPlay = true;
                return;
            }
            Initialize();
            if (brush is null || visual is null)
                return;

            StopAnimation();
            ApplyThemeColors();
            visual.Opacity = 1;
            var compositor = visual.Compositor;
            var ease = compositor.CreateCubicBezierEasingFunction(
                new Vector2(0.18f, 0.7f),
                new Vector2(0.2f, 1)
            );
            var start = compositor.CreateVector2KeyFrameAnimation();
            start.Duration = TimeSpan.FromMilliseconds(1800);
            start.InsertKeyFrame(0, new Vector2(-1.18f, 0.82f));
            start.InsertKeyFrame(0.82f, new Vector2(1.18f, 0.82f), ease);
            start.InsertKeyFrame(1, new Vector2(1.18f, 0.82f));
            var end = compositor.CreateVector2KeyFrameAnimation();
            end.Duration = start.Duration;
            end.InsertKeyFrame(0, new Vector2(-0.18f, 0.18f));
            end.InsertKeyFrame(0.82f, new Vector2(2.18f, 0.18f), ease);
            end.InsertKeyFrame(1, new Vector2(2.18f, 0.18f));

            batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            brush.StartAnimation(nameof(CompositionLinearGradientBrush.StartPoint), start);
            brush.StartAnimation(nameof(CompositionLinearGradientBrush.EndPoint), end);
            batch.Completed += OnBatchCompleted;
            batch.End();
            start.Dispose();
            end.Dispose();
            ease.Dispose();
        }

        private void Initialize()
        {
            if (visual is not null)
                return;
            var compositor = ElementCompositionPreview.GetElementVisual(button).Compositor;
            geometry = compositor.CreateRoundedRectangleGeometry();
            brush = compositor.CreateLinearGradientBrush();
            brush.StartPoint = new Vector2(-1.18f, 0.82f);
            brush.EndPoint = new Vector2(-0.18f, 0.18f);
            brush.ColorStops.Add(
                compositor.CreateColorGradientStop(0.16f, Microsoft.UI.Colors.Transparent)
            );
            brush.ColorStops.Add(
                compositor.CreateColorGradientStop(0.37f, Microsoft.UI.Colors.Transparent)
            );
            brush.ColorStops.Add(
                compositor.CreateColorGradientStop(0.63f, Microsoft.UI.Colors.Transparent)
            );
            brush.ColorStops.Add(
                compositor.CreateColorGradientStop(0.84f, Microsoft.UI.Colors.Transparent)
            );
            var shape = compositor.CreateSpriteShape(geometry);
            shape.FillBrush = brush;
            visual = compositor.CreateShapeVisual();
            visual.Shapes.Add(shape);
            visual.Opacity = 0;
            ElementCompositionPreview.SetElementChildVisual(button, visual);
            Resize();
            ApplyThemeColors();
        }

        private void ApplyThemeColors()
        {
            if (brush is null)
                return;
            var light = button.ActualTheme == ElementTheme.Light;
            var channel = light ? (byte)255 : (byte)0;
            var stops = brush.ColorStops;
            stops[0].Color = Windows.UI.Color.FromArgb(0, channel, channel, channel);
            stops[1].Color = Windows.UI.Color.FromArgb(
                light ? (byte)42 : (byte)20,
                channel,
                channel,
                channel
            );
            stops[2].Color = Windows.UI.Color.FromArgb(
                light ? (byte)158 : (byte)72,
                channel,
                channel,
                channel
            );
            stops[3].Color = Windows.UI.Color.FromArgb(0, channel, channel, channel);
        }

        private void Resize()
        {
            if (visual is null || geometry is null)
                return;
            var size = new Vector2((float)button.ActualWidth, (float)button.ActualHeight);
            visual.Size = size;
            geometry.Size = size;
            geometry.CornerRadius = new Vector2(
                (float)button.CornerRadius.TopLeft,
                (float)button.CornerRadius.TopLeft
            );
        }

        private void StopAnimation()
        {
            if (brush is not null)
            {
                brush.StopAnimation(nameof(CompositionLinearGradientBrush.StartPoint));
                brush.StopAnimation(nameof(CompositionLinearGradientBrush.EndPoint));
            }
            if (batch is not null)
            {
                batch.Completed -= OnBatchCompleted;
                batch.Dispose();
                batch = null;
            }
        }

        private void OnBatchCompleted(object sender, CompositionBatchCompletedEventArgs args)
        {
            StopAnimation();
            if (visual is not null)
                visual.Opacity = 0;
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            Initialize();
            if (pendingPlay)
            {
                pendingPlay = false;
                Play();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs args) => ReleaseComposition();

        private void OnSizeChanged(object sender, SizeChangedEventArgs args) => Resize();

        private void OnActualThemeChanged(FrameworkElement sender, object args) =>
            ApplyThemeColors();

        private void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            if (GetPlayOnPointerOver(button))
                Play();
        }

        private void OnButtonIsEnabledChanged(
            object sender,
            DependencyPropertyChangedEventArgs args
        )
        {
            if ((bool)args.NewValue && GetPlayWhenEnabled(button))
                Play();
        }

        private void ReleaseComposition()
        {
            StopAnimation();
            ElementCompositionPreview.SetElementChildVisual(button, null);
            if (visual is not null)
            {
                foreach (var shape in visual.Shapes)
                    shape.Dispose();
                visual.Dispose();
            }
            if (brush is not null)
            {
                foreach (var stop in brush.ColorStops)
                    stop.Dispose();
            }
            brush?.Dispose();
            geometry?.Dispose();
            visual = null;
            brush = null;
            geometry = null;
        }

        public void Dispose()
        {
            ReleaseComposition();
            button.Loaded -= OnLoaded;
            button.Unloaded -= OnUnloaded;
            button.SizeChanged -= OnSizeChanged;
            button.PointerEntered -= OnPointerEntered;
            button.IsEnabledChanged -= OnButtonIsEnabledChanged;
            button.ActualThemeChanged -= OnActualThemeChanged;
        }
    }
}
