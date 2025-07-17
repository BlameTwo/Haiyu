using System.Timers;
using CounterMonitor.Common;
using CounterMonitor.Handler;
using CounterMonitor.Models;
using LibreHardwareMonitor.Hardware;
using Timer = System.Timers.Timer;

namespace CounterMonitor;

public class ComputerCounter : ICounter
{
    Timer timer;
    Computer Computer { get; set; }
    public bool Pause { get; set; }
    UpdateVisitor Visitor => new UpdateVisitor();

    public int Decimal { get; set; } = 2;

    #region Event
    private ComputerCounterOutputDelegate? computerOutputHandler;

    public event ComputerCounterOutputDelegate ComputerOutput
    {
        add => computerOutputHandler += value;
        remove => computerOutputHandler -= value;
    }
    #endregion

    public void Dispose()
    {
        this.Stop();
    }

    public void Start()
    {
        if (Computer != null)
        {
            Computer.Close();
            Computer = null;
        }
        Computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
        };
        timer = new Timer();
        timer.Elapsed += Timer_Elapsed;
        timer.Interval = 1000;
        Computer.Open();
        timer.Start();
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        Computer.Accept(this.Visitor);
        IList<ComputerItem> items = new List<ComputerItem>();
        foreach (var hardware in Computer.Hardware)
        {
            if (hardware.HardwareType == HardwareType.Cpu)
            {
                foreach (var item in hardware.Sensors)
                {
                    if (item.Name == "CPU Total")
                    {
                        if (item.SensorType == SensorType.Load)
                        {
                            items.Add(
                                new()
                                {
                                    Type = HardwareType.Cpu,
                                    DisplayName = "CPU",
                                    Extension = "%",
                                    HardwareName = hardware.Name,
                                    Value = Math.Round((double)item.Value!, Decimal),
                                }
                            );
                        }
                    }
                }
            }
            if (
                hardware.HardwareType == HardwareType.GpuNvidia
                || hardware.HardwareType == HardwareType.GpuAmd
                || hardware.HardwareType == HardwareType.GpuIntel
            )
            {
                foreach (var item in hardware.Sensors)
                {
                    if (item.SensorType == SensorType.Load)
                    {
                        //GPU核心占用比例
                        if (item.Name == "D3D 3D")
                        {
                            var count =
                                items
                                    .Where(x =>
                                        x.Type == HardwareType.GpuAmd
                                        || x.Type == HardwareType.GpuIntel
                                        || x.Type == HardwareType.GpuNvidia
                                    )
                                    .Count() + 1;
                            items.Add(
                                new()
                                {
                                    Type = hardware.HardwareType,
                                    DisplayName = $"GPU{count}",
                                    Extension = "%",
                                    HardwareName = hardware.Name,
                                    Value = Math.Round((double)item.Value!, Decimal),
                                }
                            );
                        }
                    }
                }
            }
            if (hardware.HardwareType == HardwareType.Memory)
            {
                foreach (var item in hardware.Sensors)
                {
                    if (item.Name == "Memory")
                    {
                        if (item.SensorType == SensorType.Load)
                        {
                            items.Add(
                                new()
                                {
                                    Type = HardwareType.Memory,
                                    DisplayName = "RAM",
                                    Extension = "%",
                                    HardwareName = hardware.Name,
                                    Value = Math.Round((double)item.Value!, Decimal),
                                }
                            );
                        }
                    }
                }
            }
        }
        this.computerOutputHandler?.Invoke(this, items);
    }

    public void Stop()
    {
        timer.Stop();
        Computer.Close();
        timer.Dispose();
        timer = null;
        Computer = null;
    }
}
