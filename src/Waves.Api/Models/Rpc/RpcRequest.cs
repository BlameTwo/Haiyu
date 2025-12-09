using System.Text.Json.Serialization;

namespace Waves.Api.Models.Rpc;

public class RpcRequest
{
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("params")]
    public List<RpcParams> Params { get; set; }

    [JsonPropertyName("requestId")]
    public long RequestId { get; set; }
}

public class RpcParams
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}
