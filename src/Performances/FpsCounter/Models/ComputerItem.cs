using LibreHardwareMonitor.Hardware;

namespace CounterMonitor.Models;

public class ComputerItem
{
    public string DisplayName { get; set; }

    public HardwareType Type { get; set; }

    public string HardwareName { get; set; }

    public double Value { get; set; }

    public string Extension { get; set; }
}
