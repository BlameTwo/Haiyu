using System.Text.Json.Serialization;
using Waves.Api.Models.CloudGame;

namespace Waves.Api.Models.Rpc;

[JsonSerializable(typeof(RpcRequest))]
[JsonSerializable(typeof(RpcParams))]
[JsonSerializable(typeof(List<RpcParams>))]
[JsonSerializable(typeof(RpcReponse))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(RecordData))]
public partial class RpcContext:JsonSerializerContext
{
    
}
