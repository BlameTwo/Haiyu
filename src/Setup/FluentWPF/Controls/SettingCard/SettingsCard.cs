using FluentWPF.Contracts.SettingCards;
using FluentWPF.Resources;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FluentWPF.Controls;

[ContentProperty("Content")]
public class SettingsCard : Control, ISettingCardItem
{
    public string Header
    {
        get { return (string)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        "Header",
        typeof(string),
        typeof(SettingsCard),
        new PropertyMetadata("")
    );

    public FontIconEnum HeaderIcon
    {
        get { return (FontIconEnum)GetValue(HeaderIconProperty); }
        set { SetValue(HeaderIconProperty, value); }
    }

    public static readonly DependencyProperty HeaderIconProperty = DependencyProperty.Register(
        "HeaderIcon",
        typeof(FontIconEnum),
        typeof(SettingsCard),
        new PropertyMetadata(FontIconEnum.Light)
    );

    public object Description
    {
        get { return (object)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        "Description",
        typeof(object),
        typeof(SettingsCard),
        new PropertyMetadata("")
    );

    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        "Content",
        typeof(object),
        typeof(SettingsCard),
        new PropertyMetadata("")
    );

    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        "CornerRadius",
        typeof(CornerRadius),
        typeof(SettingsCard),
        new PropertyMetadata(null)
    );
}
