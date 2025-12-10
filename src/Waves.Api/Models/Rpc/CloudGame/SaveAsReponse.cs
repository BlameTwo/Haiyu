using System.Text.Json.Serialization;

namespace Waves.Api.Models.Rpc.CloudGame;

public class SaveAsReponse
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("fileSize")]
    public long FileSize { get; set; }

    [JsonPropertyName("dataCount")]
    public long DataCount { get; set; }

    [JsonPropertyName("margeTime")]
    public DateTime? MargeTime { get; set;}
}
