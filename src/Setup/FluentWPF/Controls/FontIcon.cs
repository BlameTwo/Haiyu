using FluentWPF.Resources;
using System.Windows.Controls;

namespace FluentWPF.Controls;

public class FontIcon : Control
{
    public FontIconEnum Glyph
    {
        get { return (FontIconEnum)GetValue(GlyphProperty); }
        set { SetValue(GlyphProperty, value); }
    }

    public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
        "Glyph",
        typeof(FontIconEnum),
        typeof(FontIcon),
        new PropertyMetadata(null)
    );
}
