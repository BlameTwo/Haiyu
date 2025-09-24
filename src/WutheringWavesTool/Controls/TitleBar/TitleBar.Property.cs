using Windows.Graphics;

namespace Haiyu.Controls;

/// <summary>
/// TitleBar的依赖属性
/// </summary>
partial class TitleBar
{
    /// <summary>
    /// Window基类
    /// </summary>
    public Window Window
    {
        get { return (Window)GetValue(WindowProperty); }
        set { SetValue(WindowProperty, value); }
    }

    public static readonly DependencyProperty WindowProperty = DependencyProperty.Register(
        "Window",
        typeof(Window),
        typeof(TitleBar),
        new PropertyMetadata(default, OnWindowChanged)
    );

    /// <summary>
    /// 是否拓展标题栏
    /// </summary>
    public bool IsExtendsContentIntoTitleBar
    {
        get { return (bool)GetValue(IsExtendsContentIntoTitleBarProperty); }
        set { SetValue(IsExtendsContentIntoTitleBarProperty, value); }
    }
    public static readonly DependencyProperty IsExtendsContentIntoTitleBarProperty =
        DependencyProperty.Register(
            "IsExtendsContentIntoTitleBar",
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(default)
        );

    /// <summary>
    /// 标题栏
    /// </summary>

    public object Title
    {
        get { return (object)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title",
        typeof(object),
        typeof(TitleBar),
        new PropertyMetadata(default)
    );

    /// <summary>
    /// 头部交互
    /// </summary>
    public object Header
    {
        get { return (object)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        "Header",
        typeof(object),
        typeof(TitleBar),
        new PropertyMetadata(default)
    );

    #region 对齐方式
    public HorizontalAlignment HeadHorizontalAlignment
    {
        get { return (HorizontalAlignment)GetValue(HeadHorizontalAlignmentProperty); }
        set { SetValue(HeadHorizontalAlignmentProperty, value); }
    }

    public static readonly DependencyProperty HeadHorizontalAlignmentProperty =
        DependencyProperty.Register(
            "HeadHorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

    public VerticalAlignment HeadVerticalAlignment
    {
        get { return (VerticalAlignment)GetValue(HeadVerticalAlignmentProperty); }
        set { SetValue(HeadVerticalAlignmentProperty, value); }
    }

    public static readonly DependencyProperty HeadVerticalAlignmentProperty =
        DependencyProperty.Register(
            "HeadVerticalAlignment",
            typeof(VerticalAlignment),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

    public HorizontalAlignment TitleHorizontalAlignment
    {
        get { return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty); }
        set { SetValue(TitleHorizontalAlignmentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TitleHorizontalAlignment.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleHorizontalAlignmentProperty =
        DependencyProperty.Register(
            "TitleHorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

    public VerticalAlignment TitleVerticalAlignment
    {
        get { return (VerticalAlignment)GetValue(TitleVerticalAlignmentProperty); }
        set { SetValue(TitleVerticalAlignmentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TitleVerticalAlignment.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleVerticalAlignmentProperty =
        DependencyProperty.Register(
            "TitleVerticalAlignment",
            typeof(VerticalAlignment),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

    public HorizontalAlignment FooterHorizontalAlignment
    {
        get { return (HorizontalAlignment)GetValue(FooterHorizontalAlignmentProperty); }
        set { SetValue(FooterHorizontalAlignmentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FooterHorizontalAlignment.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FooterHorizontalAlignmentProperty =
        DependencyProperty.Register(
            "FooterHorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

    public VerticalAlignment FooterVerticalAlignment
    {
        get { return (VerticalAlignment)GetValue(FooterVerticalAlignmentProperty); }
        set { SetValue(FooterVerticalAlignmentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FooterVerticalAlignment.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FooterVerticalAlignmentProperty =
        DependencyProperty.Register(
            "FooterVerticalAlignment",
            typeof(VerticalAlignment),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

    #endregion

    /// <summary>
    /// 尾部交互
    /// </summary>
    public object Footer
    {
        get { return (object)GetValue(FooterProperty); }
        set { SetValue(FooterProperty, value); }
    }

    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
        "Footer",
        typeof(object),
        typeof(TitleBar),
        new PropertyMetadata(default)
    );

    /// <summary>
    /// 标题栏类型
    /// </summary>
    public TitleBarHeightOption TitleMode
    {
        get { return (TitleBarHeightOption)GetValue(TitleModeProperty); }
        set { SetValue(TitleModeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TitleMode.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleModeProperty = DependencyProperty.Register(
        "TitleMode",
        typeof(TitleBarHeightOption),
        typeof(TitleBar),
        new PropertyMetadata(TitleBarHeightOption.Standard, OnTitleBarModeChanged)
    );

    /// <summary>
    /// 最大化按钮可见度
    /// </summary>
    public bool IsMaxButtonVisibility
    {
        get { return (bool)GetValue(IsMaxButtonVisibilityProperty); }
        set { SetValue(IsMaxButtonVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsMaxButtonVisibility.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsMaxButtonVisibilityProperty =
        DependencyProperty.Register(
            "IsMaxButtonVisibility",
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true, OnMaxButtonVisibilityChanged)
        );

    /// <summary>
    /// 最小化按钮的可见度
    /// </summary>
    public bool IsMinButtonVisibility
    {
        get { return (bool)GetValue(IsMinButtonVisibilityProperty); }
        set { SetValue(IsMinButtonVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsMinButtonVisibility.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsMinButtonVisibilityProperty =
        DependencyProperty.Register(
            "IsMinButtonVisibility",
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true, OnMinButtonVisibilityChanged)
        );

    public List<RectInt32> ContentRects
    {
        get { return (List<RectInt32>)GetValue(ContentRectsProperty); }
        set { SetValue(ContentRectsProperty, value); }
    }

    public static readonly DependencyProperty ContentRectsProperty = DependencyProperty.Register(
        "ContentRects",
        typeof(List<RectInt32>),
        typeof(TitleBar),
        new PropertyMetadata(null, OnContentRectChange)
    );
}
