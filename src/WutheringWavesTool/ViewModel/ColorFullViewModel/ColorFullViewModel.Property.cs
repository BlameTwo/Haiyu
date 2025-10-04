using Haiyu.Models.ColorFullGame;
using Haiyu.Models.Enums;

namespace Haiyu.ViewModel;

partial class ColorFullViewModel
{
    [ObservableProperty]
    public partial ObservableCollection<ColorCell> GameGrid { get; set; } = new();

    [ObservableProperty]
    public partial int GameColumsSize { get; set; } = 0;

    [ObservableProperty]
    public partial string State { get; set; }

    [ObservableProperty]
    public partial double GameSetup { get; set; } = 0.0;

    [ObservableProperty]
    public partial GameMode Mode { get; set; } = GameMode.Dyeing;


    [ObservableProperty]
    public partial ObservableCollection<AvailableColor> AvailableColors { get; set; } = new ObservableCollection<AvailableColor>
    {
        new AvailableColor(new SolidColorBrush(Colors.Red),"红色"),
        new AvailableColor(new SolidColorBrush(Colors.Green),"绿色"),
        new AvailableColor( new SolidColorBrush(Colors.Blue),"蓝色"),
        new AvailableColor( new SolidColorBrush(Colors.Yellow),"黄色"),
        new AvailableColor( new SolidColorBrush(Colors.Purple),"紫色"),
        new AvailableColor(new SolidColorBrush(Colors.Black),"石头",true)
    };

    [ObservableProperty]
    public partial string GameName { get; set; }

    [ObservableProperty]
    public partial AvailableColor SelectAvailableColor { get; set; }
}
