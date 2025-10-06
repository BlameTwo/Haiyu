using CommunityToolkit.WinUI.Helpers;

namespace Haiyu.Models.ColorFullGame;

public partial class AvailableColor : ObservableObject
{
    [ObservableProperty]
    public partial SolidColorBrush Color { get; set; }

    public AvailableColor(SolidColorBrush color, string name,bool isStone = false)
    {
        Color = color;
        Name = name;
        IsStone = isStone;
    }

    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial bool IsStone { get; set; }

    public override string ToString()
    {
        var result =  CommunityToolkit.WinUI.Helpers.ColorHelper.ToHex(this.Color.Color);
        return result;
    }

}
