namespace WutheringWavesTool.Common;

public partial class ViewModelBase : ObservableRecipient
{
    public CancellationTokenSource CTS { get; set; }

    public ViewModelBase()
    {
        CTS = new CancellationTokenSource();
        this.CursorName = AppSettings.SelectCursor == "默认" ? CursorName.None :
            AppSettings.SelectCursor == "弗糯糯" ? CursorName.FuLuoLuo :
            AppSettings.SelectCursor == "卡提西亚" ? CursorName.KaTiXiYa :
            AppSettings.SelectCursor == "守岸人" ? CursorName.ShouAnRen : CursorName.None;
    }

    [ObservableProperty]
    public partial CursorName CursorName { get; set; }
}
