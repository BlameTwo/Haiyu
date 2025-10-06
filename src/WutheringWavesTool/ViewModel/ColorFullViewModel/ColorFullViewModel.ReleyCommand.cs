using Haiyu.Helpers;
using Haiyu.Models.ColorFullGame;
using Haiyu.Models.ColorGames;
using Haiyu.Models.Enums;
using Microsoft.Windows.Storage.Pickers;

namespace Haiyu.ViewModel;


partial class ColorFullViewModel
{
    public string CurrentFile { get; private set; }

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
    void GeneratorCuston()
    {
        GameGrid.Clear();
        GameColumsSize = (int)this.BuildMaxColumn;
        this.GameGrid = ColorGameGenerator.GenerateColorGame((int)BuildMaxRow, (int)BuildMaxColumn).ToObservableCollection();
    }

    [RelayCommand]
    public async Task CellClicked(ItemsViewItemInvokedEventArgs cell)
    {
        if (this.SelectAvailableColor == null)
            return;
        if (cell.InvokedItem is ColorCell value)
        {
            TipShow.ShowMessage($"选择格子{value.Row}:{value.Column}", Symbol.Message);

            if (this.Mode == Models.Enums.ColorGameEditMode.DotDyeing)
            {
                value.IsStone = SelectAvailableColor.IsStone;
                value.CurrentColor = SelectAvailableColor.Color;
            }
            if (this.Mode == Models.Enums.ColorGameEditMode.Dyeing)
            {
                if(this.SelectAvailableColor.IsStone == true || value.IsStone == true)
                {
                    State = "石头不可以进行染色，只能点染";
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
        try
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
            info.GameObjective = this.SelectGameEndColor.ToString();
            var picker = new FileSavePicker(Instance.GetService<IAppContext<App>>().App.MainWindow.AppWindow.Id);
            picker.DefaultFileExtension = ".json";
            picker.SuggestedFileName = "保存关卡配置";
            picker.CommitButtonText = "保存配置";
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var result = await picker.PickSaveFileAsync();
            if (result == null)
                return;
            using (var fs = File.CreateText(result.Path))
            {
                await fs.WriteAsync(JsonSerializer.Serialize(info, GameContext.Default.ColorInfo));
            }
            TipShow.ShowMessage($"保存成功", Symbol.Message);
            this.CurrentFile = result.Path;
        }
        catch (Exception ex)
        {

            TipShow.ShowMessage($"保存失败{ex.Message}", Symbol.Message);
        }
    }

    [RelayCommand]
    public async Task OpenGame()
    {
        try
        {
            var gameFile = await PickersService.GetFileOpenPicker([".json"]);
            if (gameFile == null)
                return;
            var jsonObj = JsonSerializer.Deserialize(await File.ReadAllTextAsync(gameFile.Path, this.CTS.Token), GameContext.Default.ColorInfo);
            this.GameColumsSize = jsonObj.MaxColumns;
            this.GameSetup = jsonObj.Setups;
            foreach (var item in jsonObj.Cells)
            {
                item.CurrentColor = item.GetSaveColor();
            }
            this.GameGrid = jsonObj.Cells.ToObservableCollection();
            this.GameName = jsonObj.GameFile;
            this.CurrentFile = gameFile.Path;
            foreach (var item in ObjectiveColors)
            {
                if (CommunityToolkit.WinUI.Helpers.ColorHelper.ToHex(item.Color.Color) == jsonObj.GameObjective)
                {
                    SelectGameEndColor = item;
                }
            }
        }
        catch (Exception ex)
        {
            TipShow.ShowMessage("打开文件错误", Symbol.Clear);
        }
    }


    [RelayCommand]
    public async Task Reset()
    {
        if (string.IsNullOrWhiteSpace(this.CurrentFile))
        {
            TipShow.ShowMessage("请保存一个游戏文件或打开一个游戏文件", Symbol.Clear);
            return;
        }
        try
        {
            var jsonObj = JsonSerializer.Deserialize(await File.ReadAllTextAsync(CurrentFile, this.CTS.Token), GameContext.Default.ColorInfo);
            this.GameColumsSize = jsonObj.MaxColumns;
            this.GameSetup = jsonObj.Setups;
            foreach (var item in jsonObj.Cells)
            {
                item.CurrentColor = item.GetSaveColor();
            }
            this.GameGrid = jsonObj.Cells.ToObservableCollection();
            this.GameName = jsonObj.GameFile;
            foreach (var item in ObjectiveColors)
            {
                if (CommunityToolkit.WinUI.Helpers.ColorHelper.ToHex(item.Color.Color) == jsonObj.GameObjective)
                {
                    SelectGameEndColor = item;
                }
            }
        }
        catch (Exception ex)
        {
            TipShow.ShowMessage("重置色块错误", Symbol.Clear);
        }
    }

    [RelayCommand]
    public void NewFile()
    {
        this.Generator8X12();
        this.CurrentFile = null;
        this.GameSetup = 0;
        if (TipShow == null)
            return;
        TipShow.ShowMessage($"重新绘制画布", Symbol.Message);
    }
    
    [RelayCommand]
    void SetGameMode(int mode)
    {
        this.Mode = (ColorGameEditMode)mode;
        TipShow.ShowMessage($"选择操作模式:{mode}", Symbol.Message);
    }
}
