using System.Text.Json.Serialization;

namespace Waves.Api.Models.CloudGame;

public class EndLoginRequest
{
    [JsonPropertyName("loginType")]
    public int LoginType { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("deviceId")]
    public string DeviceId { get; set; }

    [JsonPropertyName("platform")]
    public string Platform { get; set; }

    [JsonPropertyName("appVersion")]
    public string AppVersion { get; set; }
}
