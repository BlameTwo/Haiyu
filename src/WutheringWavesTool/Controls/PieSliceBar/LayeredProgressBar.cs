namespace Haiyu.Controls;

public partial class LayerData : ObservableObject
{
    [ObservableProperty]
    public partial double Value { get; set; }

    [ObservableProperty]
    public partial string Label { get; set; }

    [ObservableProperty]
    public partial Brush Color { get; set; }
}

public sealed partial class LayeredProgressBar : Control
{
    public LayeredProgressBar()
    {
        SizeChanged += LayeredProgressBar_SizeChanged;
        Unloaded += LayeredProgressBar_Unloaded;
    }

    private void LayeredProgressBar_Unloaded(object sender, RoutedEventArgs e)
    {
        SizeChanged -= LayeredProgressBar_SizeChanged;
        Unloaded -= LayeredProgressBar_Unloaded;
    }

    private void LayeredProgressBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        this.UpdateLayers();
    }

    private StackPanel? container;
    private double _availableWidth;

    // 核心布局逻辑
    private void UpdateLayers()
    {
        if (container == null)
            return;
        container.Children.Clear();
        var validValues = Values?.Where(v => v.Value > 0).OrderBy(v => v.Value).ToList();

        if (validValues == null || !validValues.Any())
            return;

        // 计算有效最大值（防止数值溢出）
        double actualMax = Math.Max(validValues.Max(v => v.Value), MaxValue);
        int offsetIndex = 1;
        double lastWidth = -1;
        for (int i = 0; i < validValues.Count; i++)
        {
            var maxWidth = ActualWidth;
            double width = 0;
            if (lastWidth == -1)
            {
                width = ConvertRange(validValues[i].Value, 0, MaxValue, 0, this.ActualWidth);
            }
            else
            {
                width =
                    ConvertRange(validValues[i].Value, 0, MaxValue, 0, this.ActualWidth)
                    - lastWidth;
            }
            lastWidth = width;
            var rect = new Border
            {
                Width = width,
                Height = BarHeight,
                Background = validValues[i].Color,
                CornerRadius = new CornerRadius(0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Transitions = new TransitionCollection()
                {
                    new EntranceThemeTransition()
                    {
                        FromHorizontalOffset = 30 * offsetIndex,
                        FromVerticalOffset = 0,
                    },
                },
            };
            ToolTipService.SetToolTip(rect, validValues[i].Value);
            container.Children.Add(rect);
            offsetIndex++;
        }
    }

    // 属性变化处理
    private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as LayeredProgressBar)?.UpdateLayers();
    }

    protected override void OnApplyTemplate()
    {
        container = (StackPanel?)GetTemplateChild("LayersContainer");
        UpdateLayers();
    }

    public static double ConvertRange(
        double value,
        double originalMin,
        double originalMax,
        double targetMin,
        double targetMax
    )
    {
        double originalRange = originalMax - originalMin;
        double targetRange = targetMax - targetMin;

        if (originalRange == 0)
        {
            return (targetMin + targetMax) / 2;
        }

        return ((value - originalMin) / originalRange) * targetRange + targetMin;
    }
}
