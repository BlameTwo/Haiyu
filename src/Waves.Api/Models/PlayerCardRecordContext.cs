using System.Text.Json.Serialization;
using Waves.Api.Models.CloudGame;
using Waves.Api.Models.Record;
using Waves.Api.Models.Wrappers;

namespace Waves.Api.Models;

[JsonSerializable(typeof(Datum))]
[JsonSerializable(typeof(PlayerCard))]
[JsonSerializable(typeof(PlayerReponse))]
[JsonSerializable(typeof(List<Datum>))]
[JsonSerializable(typeof(RecordRequest))]
[JsonSerializable(typeof(FiveGroupData))]
[JsonSerializable(typeof(FiveGroupConfig))]
[JsonSerializable(typeof(FiveMap))]
[JsonSerializable(typeof(PoolList))]
[JsonSerializable(typeof(FiveGroupModel))]
[JsonSerializable(typeof(VersionPool))]
[JsonSerializable(typeof(CommunityRoleData))]
[JsonSerializable(typeof(List<CommunityRoleData>))]
[JsonSerializable(typeof(Prop))]
[JsonSerializable(typeof(Tag))]
[JsonSerializable(typeof(RecordCacheDetily))]
[JsonSerializable(typeof(RecordCardItemWrapper))]
[JsonSerializable(typeof(List<RecordCardItemWrapper>))]
[JsonSerializable(typeof(CommunityWeaponData))]
[JsonSerializable(typeof(List<CommunityWeaponData>))]
public partial class PlayerCardRecordContext : JsonSerializerContext { }

[JsonSerializable(typeof(AddUserGameServer))]
[JsonSerializable(typeof(AddUserDatum))]
[JsonSerializable(typeof(SendGameVerifyCode))]
[JsonSerializable(typeof(BindGameVerifyCode))]
[JsonSerializable(typeof(TokenData))]
public partial class BindGameContext : JsonSerializerContext { }
