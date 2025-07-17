namespace CounterMonitor.Models;

public class FpsOutput(int value, string name)
{
    public int FPS { get; set; } = value;

    public string Name { get; set; } = name;
}
