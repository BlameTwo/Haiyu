using CounterMonitor;

namespace WutheringWavesTool.Services;

public class CounterService : ICounterService
{
    public ComputerCounter ComputerCounter { get; private set; }

    public FPSCounter FpsCounter { get; private set; }

    public CounterService()
    {
        ComputerCounter = new ComputerCounter();
        FpsCounter = new FPSCounter();
    }

    public void Start()
    {
        ComputerCounter.Start();
        FpsCounter.Start();
    }

    public void ShowWindow() { }
}
