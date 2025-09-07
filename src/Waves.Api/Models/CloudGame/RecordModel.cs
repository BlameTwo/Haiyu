using System.Text.Json.Serialization;

namespace Waves.Api.Models.CloudGame;

public class RecordData
{
    [JsonPropertyName("playerId")]
    public int PlayerId { get; set; }

    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }
}

public class RecordModel
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("data")]
    public RecordData Data { get; set; }
}
