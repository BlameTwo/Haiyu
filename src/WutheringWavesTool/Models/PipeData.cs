using LiveChartsCore.Kernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haiyu.Models;

public partial class PieData : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial double[] Values { get; set; }

    [ObservableProperty]
    public partial double Offset { get; set; }

    public Func<ChartPoint, string> Formatter { get; set; } =
        point =>
        {
            var pv = point.Coordinate.PrimaryValue;
            var sv = point.StackedValue!;

            return $"{Math.Round(pv / sv.Total * 100, 2)}%";
        };
}
