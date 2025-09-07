using System.Text.Json.Serialization;
using Waves.Api.Models.CloudGame;

namespace Waves.Api.Models;

[JsonSerializable(typeof(LoginResult))]
[JsonSerializable(typeof(CloudSendSMS))]
[JsonSerializable(typeof(AccessToken))]
[JsonSerializable(typeof(AccessData))]
[JsonSerializable(typeof(PhoneTokenData))]
[JsonSerializable(typeof(PhoneTokenModel))]
[JsonSerializable(typeof(EndLoginData))]
[JsonSerializable(typeof(EndLoginRequest))]
[JsonSerializable(typeof(ExperienceCardInfo))]
[JsonSerializable(typeof(FreeTimeInfo))]
[JsonSerializable(typeof(HsstsToken))]
[JsonSerializable(typeof(PayTimeInfo))]
[JsonSerializable(typeof(EndLoginModel))]
[JsonSerializable(typeof(TimeCardInfo))]
[JsonSerializable(typeof(WalletData))]
[JsonSerializable(typeof(EndLoginReponseData))]
[JsonSerializable(typeof(EndLoginReponse))]
[JsonSerializable(typeof(RecordModel))]
[JsonSerializable(typeof(RecordData))]
[JsonSerializable(typeof(RecardQuery))]
public partial class CloundContext : JsonSerializerContext { }
