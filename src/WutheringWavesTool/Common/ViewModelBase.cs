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

    /// <summary>
    /// 闭包返回
    /// </summary>
    /// <typeparam name="T">任务结果</typeparam>
    /// <param name="task">任务本体</param>
    /// <returns>检查结果</returns>
    public async Task<(int,T?,string?)> TryInvokeAsync<T>(Task<T?> task)
        where T:class
    {
        try
        {
            var result = await task;
            return (0, result, "");
        }
        catch (OperationCanceledException OperationCanceledException)
        {
            return (-1, null,"用户取消操作");
        }
        catch (Exception ex)
        {
            return (-2, null, ex.Message);
        }
    }
}
