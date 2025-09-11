using WutheringWavesTool.Models.ColorFullGame;
using WutheringWavesTool.Models.Enums;

namespace WutheringWavesTool.ViewModel;

partial class ColorFullViewModel
{
    [ObservableProperty]
    public partial ObservableCollection<ColorCell> GameGrid { get; set; } = new();

    [ObservableProperty]
    public partial int GameColumsSize { get; set; } = 0;


    public GameMode Mode { get; private set; }

}
