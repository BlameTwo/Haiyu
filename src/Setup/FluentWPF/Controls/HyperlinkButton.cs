using System.Diagnostics;
using System.Windows.Documents;

namespace FluentWPF.Controls;

public class HyperlinkButton : ButtonBase
{
    public string NavigateUri
    {
        get { return (string)GetValue(NavigateUriProperty); }
        set { SetValue(NavigateUriProperty, value); }
    }

    // Using a DependencyProperty as the backing store for NavigateUri.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(
        "NavigateUri",
        typeof(string),
        typeof(HyperlinkButton),
        new PropertyMetadata(null)
    );

    protected override void OnClick()
    {
        if (this.NavigateUri == null)
            return;
        //调用资源管理器
        Process.Start("explorer.exe", NavigateUri);
        base.OnClick();
    }

    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(HyperlinkButton),
        new PropertyMetadata(null)
    );
}
