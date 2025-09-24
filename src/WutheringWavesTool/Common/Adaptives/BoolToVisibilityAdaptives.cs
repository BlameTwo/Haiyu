namespace Haiyu.Common.Adaptives;

public sealed class BoolToVisibilityAdaptives : IAdaptive<Visibility, bool?>
{
    public static BoolToVisibilityAdaptives Instance = new();

    public static BoolToVisibilityAdaptives Create(bool isReversal) =>
        new BoolToVisibilityAdaptives() { IsReversal = isReversal };

    public bool IsReversal { get; set; } = false;

    public bool? GetBack(Visibility forward)
    {
        if (IsReversal)
        {
            if (forward == Visibility.Visible)
                return false;
            return true;
        }
        else
        {
            if (forward == Visibility.Visible)
                return true;
            return false;
        }
    }

    public Visibility GetForward(bool? value)
    {
        if (IsReversal)
        {
            if (value == true)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }
        else
        {
            if (value == true)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
