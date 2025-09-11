using WutheringWavesTool.Models.ColorFullGame;

namespace WutheringWavesTool.ViewModel;

public sealed partial class ColorFullViewModel:ViewModelBase
{
    public ColorFullViewModel()
    {
        this.Generator8X12();
    }

    [ObservableProperty]
    public partial ObservableCollection<AvailableColor> AvailableColors { get; set; } = new ObservableCollection<AvailableColor>
    {
        new AvailableColor(new SolidColorBrush(Colors.Red),"红色"),
        new AvailableColor(new SolidColorBrush(Colors.Green),"绿色"),
        new AvailableColor( new SolidColorBrush(Colors.Blue),"蓝色"),
        new AvailableColor( new SolidColorBrush(Colors.Yellow),"黄色"),
        new AvailableColor( new SolidColorBrush(Colors.Purple),"紫色")
    };


    [ObservableProperty]
    public partial AvailableColor SelectAvailableColor { get; set; }


    [RelayCommand]
    public void CellClicked(ItemsViewItemInvokedEventArgs cell)
    {
        if (cell.InvokedItem is ColorCell value)
        {
            if(this.Mode == Models.Enums.GameMode.DotDyeing)
            {
                value.CurrentColor = SelectAvailableColor.Color;
            }
            if(this.Mode == Models.Enums.GameMode.Dyeing)
            {
                if (cell != null && !value.CurrentColor.Color.Equals(SelectAvailableColor.Color))
                {
                    FloodFill(value.Row, value.Column, value.CurrentColor.Color, SelectAvailableColor.Color.Color);

                    CheckWinCondition();
                }
            }
            
        }
    }
    private void CheckWinCondition()
    {
        if (GameGrid.Count > 0)
        {
            var firstColor = GameGrid[0].CurrentColor.Color;
            bool allSameColor = GameGrid.All(cell => cell.CurrentColor.Color.Equals(firstColor));
            if (allSameColor)
            {

            }
        }
    }
    private void FloodFill(int row, int col, Color targetColor, Color replacementColor)
    {
        if (row < 0 || row >= 8 || col < 0 || col >= 12)
            return;

        int index = row * 12 + col;
        if (index >= GameGrid.Count || !GameGrid[index].CurrentColor.Color.Equals(targetColor))
            return;
        GameGrid[index].CurrentColor = new SolidColorBrush(replacementColor);

        FloodFill(row - 1, col, targetColor, replacementColor);
        FloodFill(row + 1, col, targetColor, replacementColor);
        FloodFill(row, col - 1, targetColor, replacementColor);
        FloodFill(row, col + 1, targetColor, replacementColor);
    }
}
