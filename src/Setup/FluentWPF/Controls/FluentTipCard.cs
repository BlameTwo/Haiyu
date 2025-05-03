using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FluentWPF.Controls;

[ContentProperty("Content")]
public class FluentTipCard : Control
{
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(FluentTipCard),
        new PropertyMetadata(null)
    );



    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(object), typeof(FluentTipCard), new PropertyMetadata(0));


    public void Show(TimeSpan time,Panel OwnerPanel)
    {
        OwnerPanel.Dispatcher.Invoke(new Action(async () =>
        {
            OwnerPanel.Children.Add(this);
            Storyboard open = GetBoard(0);
            open.Begin();
            await Task.Delay(time);
            Storyboard close = GetBoard(1);
            close.FillBehavior = FillBehavior.Stop;
            close.Begin();
            await Task.Delay(TimeSpan.FromSeconds(2));
            OwnerPanel.Children.Remove(this);
        }));
    }

    private Storyboard GetBoard(int v)
    {
        TransformGroup group = new TransformGroup();
        TranslateTransform translate = new TranslateTransform() { Y = this.ActualHeight };
        group.Children.Add(translate);
        this.RenderTransform = group;
        Storyboard story = new();
        if (v == 1)
        {
            DoubleAnimation opac = new DoubleAnimation() { From = 1, To = 0, Duration = TimeSpan.FromSeconds(0.5) };
            Storyboard.SetTarget(opac, this);
            Storyboard.SetTargetProperty(opac, new("Opacity"));

            DoubleAnimation trananimation = new() { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }, From = -5, To = -100, Duration = TimeSpan.FromSeconds(0.8) };
            Storyboard.SetTargetProperty(trananimation, new("RenderTransform.(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            Storyboard.SetTarget(trananimation, this);
            story.Children.Add(opac);
            story.Children.Add(trananimation);
        }
        else if (v == 0)
        {
            DoubleAnimation opac = new DoubleAnimation() { From = 0, To = 1, Duration = TimeSpan.FromSeconds(0.5) };
            Storyboard.SetTarget(opac, this);
            Storyboard.SetTargetProperty(opac, new("Opacity"));
            DoubleAnimation trananimation = new() { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }, From = -100, To = -5, Duration = TimeSpan.FromSeconds(0.8) };
            Storyboard.SetTargetProperty(trananimation, new("RenderTransform.(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            Storyboard.SetTarget(trananimation, this);
            story.Children.Add(opac);
            story.Children.Add(trananimation);
        }
        return story;
    }
}
