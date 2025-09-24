using Haiyu.Helpers;
using Haiyu.Models.Enums;

namespace Haiyu.ViewModel;


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
    void Generator10X10()
    {
        GameGrid.Clear();
        GameColumsSize = 10;
        this.GameGrid = ColorGameGenerator.GenerateColorGame(10, 10).ToObservableCollection();
    }


    [RelayCommand]
    void Generator8X12()
    {
        GameGrid.Clear();
        GameColumsSize = 12;
        this.GameGrid = ColorGameGenerator.GenerateColorGame(8,12).ToObservableCollection();
    }

    [RelayCommand]
    void SetGameMode(int mode)
    {
        this.Mode = (GameMode)mode;
    }
}
