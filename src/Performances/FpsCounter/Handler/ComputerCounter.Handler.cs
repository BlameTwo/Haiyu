using CounterMonitor.Models;

namespace CounterMonitor.Handler;

public delegate void ComputerCounterOutputDelegate(
    object sender,
    IEnumerable<ComputerItem> computers
);
