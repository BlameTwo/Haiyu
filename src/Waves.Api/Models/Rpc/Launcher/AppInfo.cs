using System.Text.Json.Serialization;

namespace Waves.Api.Models.Rpc.Launcher;

public class AppInfo
{
    [JsonPropertyName("appVersion")]
    public string AppVersion { get; set; }
    [JsonPropertyName("rpcVersion")]
    public string RpcVersion { get; set; }

    [JsonPropertyName("webVersion")]
    public string WebVersion { get; set; }

    [JsonPropertyName("frameworkVersion")]
    public string FrameworkVersion { get; set; }

    [JsonPropertyName("sdkVersion")]
    public string SdkVersion { get; set; }
}
