using ABI.Models;
using ABIRuntime.Abstractions;
using Haiyu.Common.Contracts;

namespace Haiyu.ViewModel.DialogViewModels;

public partial class ClearMemoryViewModel : DialogViewModelBase
{
    Progress<IPrivilegedProgress<CleanMemoryProgress>>? _cleanProgress = null;
    private Task cleanTask;

    public ClearMemoryViewModel(DialogSession session)
        : base(session) { }

    [ObservableProperty]
    public partial int Progress { get; set; }

    [ObservableProperty]
    public partial string ProgressStr { get; set; }

    [RelayCommand]
    async Task InvokeClear()
    {
        bool initialized = await AppContext.ABIRuntimeService.Initialize(
            AppDomain.CurrentDomain.BaseDirectory
        );
        if (!initialized || AppContext.ABIRuntimeService.Runtime is null || !IsAlive)
        {
            Debug.WriteLine("监控运行时初始化失败。");
            return;
        }
        _cleanProgress = new Progress<IPrivilegedProgress<CleanMemoryProgress>>(
            (s) =>
            {
                if (s.Stage == PrivilegedStage.Executing && s.Data is { } data)
                {
                    Progress = s.Percentage;
                    ProgressStr = s.Message;
                }
            }
        );
        cleanTask = MonitorAsync(_cleanProgress, this.CTS);
        await cleanTask;
    }

    public async Task MonitorAsync(
        Progress<IPrivilegedProgress<CleanMemoryProgress>> progress,
        CancellationTokenSource token
    )
    {
        try
        {
            if (AppContext.ABIRuntimeService.Runtime == null)
                return;
            IPrivilegedResult<RunResult> result =
                await AppContext.ABIRuntimeService.Runtime!.InvokeAsync(
                    ABIRuntime.Contract.CleanMemoryContract,
                    new CleanMemoryRequest(true, true, true, true, true, true, true, true, "Haiyu"),
                    progress,
                    token.Token
                );

            if (!result.IsSuccess)
            {
                Debug.WriteLine($"清理失败：0x{result.StatusCode:X8} {result.Message}");
            }
        }
        catch (OperationCanceledException) when (token.Token.IsCancellationRequested)
        {
            Debug.WriteLine("");
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"硬件监控异常：{exception}");
        }
    }
}
