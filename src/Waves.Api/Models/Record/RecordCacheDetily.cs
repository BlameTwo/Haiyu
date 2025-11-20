using MemoryPack;
using System.Text.Json.Serialization;
using Waves.Api.Models.Wrappers;

namespace Waves.Api.Models.Record;


[MemoryPackable]
public partial class RecordCacheDetily
{
    public required string Name { get; set; }

    public DateTime Time { get; set; }

    /// <summary>
    /// 角色活动
    /// </summary>
    public IList<RecordCardItemWrapper>? RoleActivityItems { get; set; } = [];

    /// <summary>
    /// 武器活动
    /// </summary>
    public IList<RecordCardItemWrapper>? WeaponsActivityItems { get; set; } = [];

    /// <summary>
    /// 角色常驻
    /// </summary>
    public IList<RecordCardItemWrapper>? RoleResidentItems { get; set; } = [];

    /// <summary>
    /// 武器常驻
    /// </summary>
    public IList<RecordCardItemWrapper>? WeaponsResidentItems { get; set; } = [];

    /// <summary>
    /// 新手唤取
    /// </summary>
    public IList<RecordCardItemWrapper>? BeginnerItems { get; set; } = [];

    /// <summary>
    /// 新手自选感恩
    /// </summary>
    public IList<RecordCardItemWrapper>? BeginnerChoiceItems { get; set; } = [];

    /// <summary>
    /// 感恩定向
    /// </summary>
    public IList<RecordCardItemWrapper>? GratitudeOrientationItems { get; set; } = [];

    /// <summary>
    /// 角色新旅
    /// </summary>
    public IList<RecordCardItemWrapper>? RoleJourneyItems { get; set; } = [];

    /// <summary>
    /// 武器新旅
    /// </summary>
    public IList<RecordCardItemWrapper>? WeaponJourneyItems { get; set; } = [];
}
