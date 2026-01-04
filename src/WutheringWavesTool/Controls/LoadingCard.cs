namespace Haiyu.Controls;

/// <summary>
/// 加载卡片
/// </summary>
[TemplateVisualState(GroupName ="LoadingStatus",Name ="Loading")]
[TemplateVisualState(GroupName ="LoadingStatus",Name ="Loaded")]
[TemplateVisualState(GroupName ="LoadingStatus",Name ="NULL")]
[TemplatePart(Name = "PART_LoadingAction", Type = typeof(ProgressRing))]
[TemplatePart(Name = "PART_LoadingCard", Type = typeof(Border))]
public sealed partial class LoadingCard : ContentControl
{
    public LoadingCard()
    {
        this.Loaded+= (s, e) =>
        {
            UpdateStatus();
        };
        this.Unloaded+= (s, e) =>
        {
            UpdateStatus();
        };
    }
    private Border PART_LoadingCard { get; set; }
    private ProgressRing PART_LoadingAction { get; set; }
    public string Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        nameof(Message),
        typeof(string),
        typeof(LoadingCard),
        new PropertyMetadata("Refresh Data……")
    );

    protected override void OnApplyTemplate()
    {
        this.PART_LoadingCard = (Border)GetTemplateChild("PART_LoadingCard");
        this.PART_LoadingAction = (ProgressRing)GetTemplateChild("PART_LoadingAction");
        base.OnApplyTemplate();
    }

    public bool IsLoading
    {
        get { return (bool)GetValue(IsLoadingProperty); }
        set { SetValue(IsLoadingProperty, value); }
    }

    public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
        nameof(IsLoading),
        typeof(bool),
        typeof(LoadingCard),
        new PropertyMetadata(false, OnLoadingChanged)
    );

    private static void OnLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as LoadingCard).UpdateStatus();
    }

    public void UpdateStatus() 
    {
        if (this.IsLoading)
        {
            VisualStateManager.GoToState(this, "Loading", true);
        }
        else
        {
            VisualStateManager.GoToState(this, "Loaded", true);
        }
    }
}
