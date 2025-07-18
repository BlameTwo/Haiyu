using CounterMonitor;

namespace WutheringWavesTool.Services.Contracts;

public interface ICounterService
{
    public ComputerCounter ComputerCounter { get; }

    public FPSCounter FpsCounter { get; }

    void ShowWindow();
    void Start();
    void Stop();
}
