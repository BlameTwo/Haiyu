using Haiyu.Helpers;
using Haiyu.Models.ColorFullGame;
using Haiyu.Models.ColorGames;
using Haiyu.Models.Enums;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage.Pickers;

namespace Haiyu.ViewModel;


partial class ColorFullViewModel
{
    public object CurrentFile { get; private set; }

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
    public async Task CellClicked(ItemsViewItemInvokedEventArgs cell)
    {
        TipShow.ShowMessage("触发选中", Symbol.Clear);
        if (this.SelectAvailableColor == null)
            return;
        if (cell.InvokedItem is ColorCell value)
        {
            TipShow.ShowMessage($"选择格子{value.Row}:{value.Column}", Symbol.Clear);
            if (this.Mode == Models.Enums.GameMode.DotDyeing)
            {
                TipShow.ShowMessage($"点燃模式", Symbol.Clear);
                value.IsStone = SelectAvailableColor.IsStone;
                value.CurrentColor = SelectAvailableColor.Color;
            }
            if (this.Mode == Models.Enums.GameMode.Dyeing)
            {
                TipShow.ShowMessage($"染色模式", Symbol.Clear);
                if(this.SelectAvailableColor.IsStone == true || value.IsStone == true)
                {
                    State = "石头可不进行染色";
                    return;
                }
                if (cell != null && !value.CurrentColor.Color.Equals(SelectAvailableColor.Color))
                {
                    await FloodFill(value.Row, value.Column, SelectAvailableColor.Color.Color);
                    CheckWinCondition();
                }
            }
        }
    }

    [RelayCommand]
    public void FullBlackColor()
    {
        foreach (var item in this.GameGrid)
        {
            if(item.CurrentColor.Color == Colors.Gray)
            {
                item.CurrentColor = this.SelectAvailableColor.Color;
                item.IsStone = this.SelectAvailableColor.IsStone;
            }
        }
    }

    [RelayCommand]
    public void ClearBoard()
    {
        foreach (var item in this.GameGrid)
        {
            item.CurrentColor = new SolidColorBrush(Colors.Gray);
            item.IsStone = false;
        }
    }

    [RelayCommand]
    public async Task SaveGame()
    {

        foreach (var item in this.GameGrid)
        {
            item.SetSaveColor();
        }
        ColorInfo info = new ColorInfo();
        info.MaxColumns = this.GameColumsSize;
        info.Cells = this.GameGrid.ToList();
        info.Setups = GameSetup;
        info.GameMode = this.Mode;
        info.GameFile = this.GameName;
        var picker = new FileSavePicker(Instance.GetService<IAppContext<App>>().App.MainWindow.AppWindow.Id);
        picker.DefaultFileExtension = ".json";
        picker.SuggestedFileName = "保存关卡配置";
        picker.CommitButtonText = "保存配置";
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        var result = await picker.PickSaveFileAsync();
        if (result == null)
            return;
        using(var fs = File.CreateText(result.Path))
        {
            await fs.WriteAsync(JsonSerializer.Serialize(info, GameContext.Default.ColorInfo));
        }    
    }

    [RelayCommand]
    public async Task OpenGame()
    {
        var gameFile = await PickersService.GetFileOpenPicker([".json"]);
        if (gameFile == null)
            return;
        var jsonObj = JsonSerializer.Deserialize(await File.ReadAllTextAsync(gameFile.Path, this.CTS.Token),GameContext.Default.ColorInfo);
        this.GameColumsSize = jsonObj.MaxColumns;
        this.GameSetup = jsonObj.Setups;
        foreach (var item in jsonObj.Cells)
        {
            item.CurrentColor =  item.GetSaveColor();
        }
        this.GameGrid = jsonObj.Cells.ToObservableCollection();
        this.GameName = jsonObj.GameFile;
        this.CurrentFile = gameFile.Path;
    }

    [RelayCommand]
    public void NewFile()
    {
        this.Generator8X12();
        this.CurrentFile = null;
        this.GameSetup = 0;
    }
    
    [RelayCommand]
    void SetGameMode(int mode)
    {
        this.Mode = (GameMode)mode;
    }
}
