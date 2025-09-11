namespace WutheringWavesTool.Models.ColorFullGame;

public partial class AvailableColor : ObservableObject
{
    [ObservableProperty]
    public partial SolidColorBrush Color { get; set; }

    public AvailableColor(SolidColorBrush color, string name)
    {
        Color = color;
        Name = name;
    }

    [ObservableProperty]
    public partial string Name { get; set; }
}
