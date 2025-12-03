using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using MemoryPack;
using Waves.Api.Models.CloudGame;

namespace Waves.Api.Models.Wrappers;

[MemoryPackable]
public partial class RecordCardItemWrapper : ObservableObject
{
    [JsonConstructor]
    [MemoryPackConstructor]
    public RecordCardItemWrapper() { }

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
    [JsonPropertyName("cardPoolType")]
    public partial string CardPoolType { get; set; }

    [ObservableProperty]
    [JsonPropertyName("resourceId")]
    public partial int ResourceId { get; set; }

    [ObservableProperty]
    [JsonPropertyName("qualityLevel")]
    public partial int QualityLevel { get; set; }

    [ObservableProperty]
    [JsonPropertyName("resourceType")]
    public partial string ResourceType { get; set; }

    [ObservableProperty]
    [JsonPropertyName("name")]
    public partial string Name { get; set; }

    [ObservableProperty]
    [JsonPropertyName("count")]
    public partial int Count { get; set; }

    [ObservableProperty]
    [JsonPropertyName("time")]
    public partial string Time { get; set; }

    [JsonPropertyName("recordTime")]
    public DateTime RecordTime { get; }

    [JsonIgnore]
    public int Day => RecordTime.Day;
    [JsonIgnore]
    public int Month => RecordTime.Month;
    [JsonIgnore]
    public int Year => RecordTime.Year;
}
