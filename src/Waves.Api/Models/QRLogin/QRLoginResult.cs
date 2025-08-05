using System.Text.Json.Serialization;

namespace Waves.Api.Models.QRLogin;

public class QRLoginResult
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("data")]
    public bool Data { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
