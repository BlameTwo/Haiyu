using MemoryPack;
using System.Text.Json.Serialization;
using Waves.Api.Models.Wrappers;

namespace Waves.Api.Models.Record;


[MemoryPackable]
public partial class RecordCacheDetily
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }


    [JsonPropertyName("time")]

    public DateTime Time { get; set; }

    /// <summary>
    /// 角色活动
    /// </summary>
    [JsonPropertyName("roleActivityItems")]
    public IList<RecordCardItemWrapper>? RoleActivityItems { get; set; } = [];

    /// <summary>
    /// 武器活动
    /// </summary>
    [JsonPropertyName("weaponsActivityItems")]
    public IList<RecordCardItemWrapper>? WeaponsActivityItems { get; set; } = [];

    /// <summary>
    /// 角色常驻
    /// </summary>
    [JsonPropertyName("roleResidentItems")]
    public IList<RecordCardItemWrapper>? RoleResidentItems { get; set; } = [];

    /// <summary>
    /// 武器常驻
    /// </summary>
    [JsonPropertyName("weaponsResidentItems")]
    public IList<RecordCardItemWrapper>? WeaponsResidentItems { get; set; } = [];

    /// <summary>
    /// 新手唤取
    /// </summary>
    [JsonPropertyName("beginnerItems")]
    public IList<RecordCardItemWrapper>? BeginnerItems { get; set; } = [];

    /// <summary>
    /// 新手自选感恩
    /// </summary>
    [JsonPropertyName("beginnerChoiceItems")]
    public IList<RecordCardItemWrapper>? BeginnerChoiceItems { get; set; } = [];

    /// <summary>
    /// 感恩定向
    /// </summary>
    [JsonPropertyName("gratitudeOrientationItems")]
    public IList<RecordCardItemWrapper>? GratitudeOrientationItems { get; set; } = [];

    /// <summary>
    /// 角色新旅
    /// </summary>
    [JsonPropertyName("roleJourneyItems")]
    public IList<RecordCardItemWrapper>? RoleJourneyItems { get; set; } = [];

    /// <summary>
    /// 武器新旅
    /// </summary>
    [JsonPropertyName("weaponJourneyItems")]
    public IList<RecordCardItemWrapper>? WeaponJourneyItems { get; set; } = [];
}

[JsonSerializable(typeof(RecordCacheDetily))]
[JsonSerializable(typeof(RecordCardItemWrapper))]
[JsonSerializable(typeof(List<RecordCardItemWrapper>))]
public sealed partial class RecordCacheDetilyContext:JsonSerializerContext
{

}
