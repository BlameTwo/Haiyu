namespace Haiyu.Services.Contracts;

/// <summary>
/// 启动任务
/// </summary>
public interface ILauncherTaskService
{
    public Task RunAsync(CancellationToken token);
}
