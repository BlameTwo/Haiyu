using MemoryPack;
using System.Text.Json.Serialization;

namespace Waves.Api.Models.CloudGame;

public class PlayerCard
{
    [JsonPropertyName("playerId")]
    public string PlayerId { get; set; }

    [JsonPropertyName("cardPoolId")]
    public string CardPoolId { get; set; }

    [JsonPropertyName("cardPoolType")]
    public int CardPoolType { get; set; }

    [JsonPropertyName("serverId")]
    public string ServerId { get; set; }

    [JsonPropertyName("languageCode")]
    public string LanguageCode { get; set; }

    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }
}

public class PlayerReponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public List<Datum> Data { get; set; }
}

[MemoryPackable]
public partial class Datum
{
    [JsonPropertyName("cardPoolType")]
    public string CardPoolType { get; set; }

    [JsonPropertyName("resourceId")]
    public int ResourceId { get; set; }

    [JsonPropertyName("qualityLevel")]
    public int QualityLevel { get; set; }

    [JsonPropertyName("resourceType")]
    public string ResourceType { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; }
}
