using Haiyu.Models.ColorFullGame;
using Haiyu.Models.ColorGames;
using System.Threading.Tasks;

namespace Haiyu.ViewModel;

public sealed partial class ColorFullViewModel:ViewModelBase
{
    public ColorFullViewModel(ITipShow tipShow,IPickersService pickersService)
    {
        TipShow = tipShow;
        PickersService = pickersService;
        this.Mode = Models.Enums.GameMode.Game;
        NewFile();
    }

    public ITipShow TipShow { get; }
    public IPickersService PickersService { get; }

    private void CheckWinCondition()
    {
        if (GameGrid.Count > 0)
        {
            var isNotStone = GameGrid.Where(x => !x.IsStone);
            var result = isNotStone.All(x => x.CurrentColor.Color.Equals(GameGrid[0].CurrentColor.Color));
            this.State = "染色完成！";
        }
    }
    private async Task FloodFill(int row, int col, Color targetColor, Color replacementColor)
    {
        if (row < 0 || row >= 8 || col < 0 || col >= 12)
            return;
        int index = row * 12 + col;
        if (index >= GameGrid.Count || !GameGrid[index].CurrentColor.Color.Equals(targetColor))
            return;
        if (!GameGrid[index].IsStone)
        {
            GameGrid[index].CurrentColor = new SolidColorBrush(replacementColor);
            await Task.Delay(20);
        }
        await FloodFill(row - 1, col, targetColor, replacementColor);
        await FloodFill(row + 1, col, targetColor, replacementColor);
        await FloodFill(row, col - 1, targetColor, replacementColor);
        await FloodFill(row, col + 1, targetColor, replacementColor);
    }
}
