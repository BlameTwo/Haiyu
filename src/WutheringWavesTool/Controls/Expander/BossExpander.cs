using CommunityToolkit.WinUI.Controls;

namespace Haiyu.Controls;

public partial class BossExpander : SettingsExpander
{
    public ImageSource HeanderBackground
    {
        get { return (ImageSource)GetValue(HeanderBackgroundProperty); }
        set { SetValue(HeanderBackgroundProperty, value); }
    }

    public static readonly DependencyProperty HeanderBackgroundProperty =
        DependencyProperty.Register(
            "HeanderBackground",
            typeof(ImageSource),
            typeof(BossExpander),
            new PropertyMetadata(null)
        );
}
