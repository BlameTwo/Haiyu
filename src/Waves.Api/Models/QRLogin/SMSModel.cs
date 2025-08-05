using System.Text.Json.Serialization;

namespace Waves.Api.Models.QRLogin;

public class Data
{
    [JsonPropertyName("geeTest")]
    public bool GeeTest { get; set; }
}

public class SMSModel
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}