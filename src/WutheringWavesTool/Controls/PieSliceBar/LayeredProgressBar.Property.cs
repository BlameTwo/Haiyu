namespace Haiyu.Controls;

public partial class LayeredProgressBar
{
    // 依赖属性
    public IEnumerable<LayerData> Values
    {
        get => (IEnumerable<LayerData>)GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }
    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
        nameof(Values),
        typeof(IEnumerable<LayerData>),
        typeof(LayeredProgressBar),
        new PropertyMetadata(null, OnDataChanged)
    );

    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
        "ItemTemplate",
        typeof(DataTemplate),
        typeof(LayeredProgressBar),
        new PropertyMetadata(null)
    );

    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }
    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
        nameof(MaxValue),
        typeof(double),
        typeof(LayeredProgressBar),
        new PropertyMetadata(100.0)
    );

    public double BarHeight
    {
        get => (double)GetValue(BarHeightProperty);
        set => SetValue(BarHeightProperty, value);
    }
    public static readonly DependencyProperty BarHeightProperty = DependencyProperty.Register(
        nameof(BarHeight),
        typeof(double),
        typeof(LayeredProgressBar),
        new PropertyMetadata(24.0)
    );
}
