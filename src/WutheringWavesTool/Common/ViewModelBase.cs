using Waves.Core.Services;

namespace Haiyu.Common;

public partial class ViewModelBase : ObservableRecipient, IDisposable
{
    public CancellationTokenSource CTS { get; set; }

    public ViewModelBase()
    {
        CTS = new CancellationTokenSource();
        this.CursorName =
            AppSettings.SelectCursor == "默认" ? CursorName.None
            : AppSettings.SelectCursor == "弗糯糯" ? CursorName.FuLuoLuo
            : AppSettings.SelectCursor == "卡提西亚" ? CursorName.KaTiXiYa
            : AppSettings.SelectCursor == "守岸人" ? CursorName.ShouAnRen
            : CursorName.None;
        this.Logger = Instance.Service.GetRequiredKeyedService<LoggerService>("AppLog");
    }

    [ObservableProperty]
    public partial CursorName CursorName { get; set; }
    public LoggerService Logger { get; }

    /// <summary>
    /// 闭包返回
    /// </summary>
    /// <typeparam name="T">任务结果</typeparam>
    /// <param name="task">任务本体</param>
    /// <returns>检查结果</returns>
    public async Task<(int Code, T? Result, string? Message)> TryInvokeAsync<T>(
        Func<Task<T?>> taskFactory
    )
    {
        try
        {
            var result = await taskFactory();
            return (0, result, null);
        }
        catch (OperationCanceledException)
        {
            return (-1, default(T), "用户取消操作");
        }
        catch (Exception ex)
            when (ex is not StackOverflowException && ex is not OutOfMemoryException)
        {
            return (-2, default(T), ex.Message ?? "操作失败");
        }
    }

    public virtual void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        this.CTS.Cancel();
        this.CTS.Dispose();
    }
}
