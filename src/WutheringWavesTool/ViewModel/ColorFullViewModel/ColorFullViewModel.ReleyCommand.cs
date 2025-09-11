using WutheringWavesTool.Helpers;
using WutheringWavesTool.Models.Enums;

namespace WutheringWavesTool.ViewModel;


partial class ColorFullViewModel
{

    [RelayCommand]
    void Generator6X6()
    {
        GameGrid.Clear();
        GameColumsSize = 6;
        this.GameGrid = ColorGameGenerator.GenerateColorGame(6,6).ToObservableCollection();
    }


    [RelayCommand]
    void Generator8X12()
    {
        GameGrid.Clear();
        GameColumsSize = 12;
        this.GameGrid = ColorGameGenerator.GenerateColorGame(8,12).ToObservableCollection();
    }

    [RelayCommand]
    void SetGameMode(GameMode mode)
    {
        this.Mode = mode;
    }
}
