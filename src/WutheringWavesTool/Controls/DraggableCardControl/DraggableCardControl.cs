using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Animations;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using System.Numerics;
using System.Windows.Input;

namespace Haiyu.Controls;

/// <summary>
/// 可拖动的卡片控件.
/// </summary>
[ContentProperty(Name = nameof(Content))]
public sealed partial class DraggableCardControl : Control
{
    /// <summary>
    /// 是否鼠标悬停在控件上.
    /// </summary>
    public static readonly DependencyProperty IsPointerOverProperty = DependencyProperty.Register(
        nameof(IsPointerOver),
        typeof(bool),
        typeof(DraggableCardControl),
        new PropertyMetadata(default)
    );

    /// <summary>
    /// 是否按下.
    /// </summary>
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(DraggableCardControl),
        new PropertyMetadata(default)
    );

    /// <summary>
    /// 命令依赖属性.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(DraggableCardControl),
        new PropertyMetadata(default)
    );

    /// <summary>
    /// 内容依赖属性.
    /// </summary>
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(object),
        typeof(DraggableCardControl),
        new PropertyMetadata(default)
    );

    private const float PointerOverOffsetY = -4f;

    private static readonly TimeSpan _pointerOverShadowDuration = TimeSpan.FromMilliseconds(240);
    private static readonly TimeSpan _pressedShadowDuration = TimeSpan.FromMilliseconds(50);
    private static readonly TimeSpan _restShadowDuration = TimeSpan.FromMilliseconds(250);

    private Compositor _compositor;
    private FrameworkElement _shadowContainer;
    private AttachedShadowBase _initialShadow;
    private bool _loaded;
    private bool _templateApplied;
    private bool _shadowCreated;
    private bool _shouldDestroyShadow;
    private long _pointerOverToken;
    private long _pressedToken;

    /// <summary>
    /// Initializes a new instance of the <see cref="CardControl"/> class.
    /// </summary>
    public DraggableCardControl()
    {
        DefaultStyleKey = typeof(DraggableCardControl);
        _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        Loaded += DraggableCardControl_Loaded;
        Unloaded += DraggableCardControl_Unloaded;
    }

    private void DraggableCardControl_Unloaded(object sender, RoutedEventArgs e)
    {
        UnregisterPropertyChangedCallback(IsPointerOverProperty, _pointerOverToken);
        UnregisterPropertyChangedCallback(IsPressedProperty, _pressedToken);
        _loaded = false;
        _initialShadow = default;

        DestroyShadow();
        this.Loaded -= DraggableCardControl_Loaded;
        this.Unloaded -= DraggableCardControl_Unloaded;
    }

    private void DraggableCardControl_Loaded(object sender, RoutedEventArgs e)
    {
        _pointerOverToken = RegisterPropertyChangedCallback(
            IsPointerOverProperty,
            OnButtonStateChanged
        );
        _pressedToken = RegisterPropertyChangedCallback(IsPressedProperty, OnButtonStateChanged);
        _loaded = true;

        ApplyShadowAnimation();
    }

    /// <summary>
    /// 是否鼠标悬停在控件上.
    /// </summary>
    public bool IsPointerOver
    {
        get => (bool)GetValue(IsPointerOverProperty);
        set => SetValue(IsPointerOverProperty, value);
    }

    /// <summary>
    /// 是否按下.
    /// </summary>
    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        set => SetValue(IsPressedProperty, value);
    }

    /// <summary>
    /// 命令.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// 内部内容.
    /// </summary>
    public object Content
    {
        get => (object)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate()
    {
        _shadowContainer = GetTemplateChild("ShadowContainer") as FrameworkElement;
        ElementCompositionPreview.SetIsTranslationEnabled(_shadowContainer, true);

        _initialShadow = CommunityToolkit.WinUI.Effects.GetShadow(_shadowContainer);
        _templateApplied = true;
        ApplyShadowAnimation();
    }

    /// <inheritdoc/>
    protected override void OnPointerEntered(PointerRoutedEventArgs e) => IsPointerOver = true;

    /// <inheritdoc/>
    protected override void OnPointerExited(PointerRoutedEventArgs e)
    {
        IsPointerOver = false;
        IsPressed = false;
    }

    /// <inheritdoc/>
    protected override void OnPointerMoved(PointerRoutedEventArgs e) => IsPointerOver = true;

    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerRoutedEventArgs e)
    {
        var pointerPoint = e.GetCurrentPoint(this);
        var properties = pointerPoint.Properties;
        var isLeftButtonPressed = properties.IsLeftButtonPressed;
        if (isLeftButtonPressed)
        {
            Focus(FocusState.Pointer);
            IsPressed = true;
        }
    }

    /// <inheritdoc/>
    protected override void OnPointerReleased(PointerRoutedEventArgs e) => IsPressed = false;

    /// <inheritdoc/>
    protected override void OnPointerCanceled(PointerRoutedEventArgs e)
    {
        IsPressed = false;
        IsPointerOver = false;
    }

    /// <inheritdoc/>
    protected override void OnTapped(TappedRoutedEventArgs e) => Command?.Execute(default);

    private void OnButtonStateChanged(DependencyObject sender, DependencyProperty dp) =>
        ApplyShadowAnimation();

    private void CreateShadow()
    {
        if (_shadowCreated || !_loaded || !_templateApplied)
        {
            return;
        }

        CommunityToolkit.WinUI.Effects.SetShadow(_shadowContainer, _initialShadow);
        var shadowContext = _initialShadow.GetElementContext(_shadowContainer);
        shadowContext.CreateResources();

        if (shadowContext.Shadow is DropShadow dropShadow)
        {
            dropShadow.Offset = GetShadowOffset();
            dropShadow.BlurRadius = GetShadowRadius();
            dropShadow.Opacity = GetShadowOpacity();
        }

        _shadowCreated = true;
    }

    private void DestroyShadow()
    {
        if (!_shadowCreated)
        {
            return;
        }

        _shadowCreated = false;
        CommunityToolkit.WinUI.Effects.SetShadow(_shadowContainer, default);
        _compositor = default;
    }

    private void ApplyShadowAnimation()
    {
        if (!_templateApplied)
        {
            return;
        }

        var duration =
            IsPressed ? _pressedShadowDuration
            : IsPointerOver ? _pointerOverShadowDuration
            : _restShadowDuration;
        var offset =
            IsPressed ? -2f
            : IsPointerOver ? PointerOverOffsetY
            : 0f;
#pragma warning disable VSTHRD103 // Call async methods when in an async method
        AnimationBuilder
            .Create()
            .Translation(
                Axis.Y,
                offset,
                duration: duration,
                easingMode: Microsoft.UI.Xaml.Media.Animation.EasingMode.EaseInOut
            )
            .Start(this);
#pragma warning restore VSTHRD103 // Call async methods when in an async method
        var shadowOpacity = GetShadowOpacity();
        var shadowRadius = GetShadowRadius();
        var shadowOffset = GetShadowOffset();

        _shouldDestroyShadow = shadowOpacity <= 0;
        if (!_shouldDestroyShadow)
        {
            CreateShadow();
        }

        var shadowContext = _initialShadow.GetElementContext(_shadowContainer);
        if (shadowContext.Shadow is DropShadow dropShadow)
        {
            using var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            var shadowAnimationGroup = _compositor.CreateAnimationGroup();
            shadowAnimationGroup.Add(
                _compositor.CreateScalarKeyFrameAnimation(
                    nameof(DropShadow.BlurRadius),
                    shadowRadius,
                    duration: duration
                )
            );
            shadowAnimationGroup.Add(
                _compositor.CreateVector3KeyFrameAnimation(
                    nameof(DropShadow.Offset),
                    shadowOffset,
                    duration: duration
                )
            );
            shadowAnimationGroup.Add(
                _compositor.CreateScalarKeyFrameAnimation(
                    nameof(DropShadow.Opacity),
                    shadowOpacity,
                    duration: duration
                )
            );
            dropShadow.StartAnimationGroup(shadowAnimationGroup);

            if (_shouldDestroyShadow)
            {
                DestroyShadow();
            }
        }
    }

    private float GetShadowRadius() =>
        IsPointerOver ? 12f
        : IsPressed ? 2f
        : 6f;

    private float GetShadowOpacity() =>
        IsPointerOver ? 0.14f
        : IsPressed ? 0.09f
        : 0.02f;

    private Vector3 GetShadowOffset() =>
        new Vector3(
            0,
            IsPointerOver ? 4f
                : IsPressed ? 0f
                : 2f,
            0
        );

    private void ChangeState()
    {
        if (!IsEnabled)
        {
            VisualStateManager.GoToState(this, "Disabled", false);
        }
        else if (IsPressed)
        {
            VisualStateManager.GoToState(this, "Pressed", false);
        }
        else if (IsPointerOver)
        {
            VisualStateManager.GoToState(this, "PointerOver", false);
        }
        else
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }
    }
}
