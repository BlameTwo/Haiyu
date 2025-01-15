﻿using CommunityToolkit.Mvvm.ComponentModel;
using Waves.Api.Models.Record;

namespace Waves.Api.Models.Wrappers;

public partial class RecordCardItemWrapper : ObservableObject
{
    public RecordCardItemWrapper(Datum datum)
    {
        CardPoolType = datum.CardPoolType;
        ResourceId = datum.ResourceId;
        QualityLevel = datum.QualityLevel;
        ResourceType = datum.ResourceType;
        Name = datum.Name;
        Count = datum.Count;
        Time = datum.Time;
        this.RecordTime = DateTime.Parse(Time);
    }

    [ObservableProperty]
    public partial string CardPoolType { get; set; }

    [ObservableProperty]
    public partial int ResourceId { get; set; }

    [ObservableProperty]
    public partial int QualityLevel { get; set; }

    [ObservableProperty]
    public partial string ResourceType { get; set; }

    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial int Count { get; set; }

    [ObservableProperty]
    public partial string Time { get; set; }

    public DateTime RecordTime { get; }

    public int Day => RecordTime.Day;
    public int Month => RecordTime.Month;
    public int Year => RecordTime.Year;
}
