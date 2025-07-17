namespace CounterMonitor.Common;

/// <summary>
/// 计数器
/// </summary>
public interface ICounter : IDisposable
{
    public void Start();

    public void Stop();

    public bool Pause { get; set; }
}
