using System;
using WutheringWavesTool.Models.ColorFullGame;
using WutheringWavesTool.ViewModel;

namespace WutheringWavesTool.Helpers;

public static class ColorGameGenerator
{
    public static List<ColorCell> GenerateColorGame(int rows, int columns)
    {
        var gameGrid = new List<ColorCell>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                gameGrid.Add(new ColorCell
                {
                    R = 0,
                    G = 0,
                    B = 0,
                    CurrentColor = new SolidColorBrush(Colors.Black),
                    Row = row,
                    Column = col
                });
            }
        }
        return gameGrid;
    }
}
