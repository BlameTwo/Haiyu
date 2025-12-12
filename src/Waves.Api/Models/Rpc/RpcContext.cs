using System.Text.Json.Serialization;
using Waves.Api.Models.CloudGame;
using Waves.Api.Models.Rpc.CloudGame;
using Waves.Api.Models.Rpc.Launcher;

namespace Waves.Api.Models.Rpc;

[JsonSerializable(typeof(RpcRequest))]
[JsonSerializable(typeof(RpcParams))]
[JsonSerializable(typeof(List<RpcParams>))]
[JsonSerializable(typeof(RpcReponse))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(RecordData))]
[JsonSerializable(typeof(SaveAsReponse))]
[JsonSerializable(typeof(AppInfo))]
public partial class RpcContext:JsonSerializerContext
{
    
}
