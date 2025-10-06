namespace Haiyu.ViewModel;

public sealed partial class ColorFullViewModel : ViewModelBase
{
    public ColorFullViewModel([FromKeyedServices("Cache")]ITipShow tipShow, IPickersService pickersService)
    {
        TipShow = tipShow;
        PickersService = pickersService;
        NewFile();
    }

    public ITipShow TipShow { get; }
    public IPickersService PickersService { get; }

    private void CheckWinCondition()
    {
        if (GameGrid.Count > 0 || this.SelectGameEndColor != null)
        {
            var isNotStone = GameGrid.Where(x => !x.IsStone);
            if (isNotStone.Any())
            {
                var result = isNotStone.All(x => AreColorsEqual(x.CurrentColor.Color, this.SelectGameEndColor.Color.Color));
                TipShow.ShowMessage($"染色结果:{result}", Symbol.Message);
            }
        }
    }

    private async Task FloodFill(int row, int col, Color replacementColor)
    {
        var targetCell = GameGrid.FirstOrDefault(x => x.Row == row && x.Column == col);
        if (targetCell == null)
            return;

        var oldColor = (targetCell.CurrentColor as SolidColorBrush)?.Color ?? Colors.Gray;

        if (oldColor == this.SelectAvailableColor.Color.Color)
            return;

        Queue<(int r, int c)> queue = new();
        queue.Enqueue((row, col));

        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            var (r, c) = queue.Dequeue();
            var cell = GameGrid.FirstOrDefault(x => x.Row == r && x.Column == c);
            if (cell == null)
                continue;
            if ((cell.CurrentColor as SolidColorBrush)?.Color != oldColor || cell.IsStone)
                continue;
            cell.CurrentColor = new SolidColorBrush(this.SelectAvailableColor.Color.Color);
            
            for (int i = 0; i < 4; i++)
            {
                int nr = r + dr[i];
                int nc = c + dc[i];
                if (nr < 0 || nc < 0 || nr >= GameColumsSize || nc >= GameColumsSize)
                    continue;
                var neighbor = GameGrid.FirstOrDefault(x => x.Row == nr && x.Column == nc);
                if (neighbor != null && (neighbor.CurrentColor as SolidColorBrush)?.Color == oldColor && !neighbor.IsStone)
                    queue.Enqueue((nr, nc));
            }
            await Task.Delay(10);
        }
        CheckWinCondition();
    }

    private bool AreColorsEqual(Color color1, Color color2)
    {
        return color1.A == color2.A &&
               color1.R == color2.R &&
               color1.G == color2.G &&
               color1.B == color2.B;
    }

}