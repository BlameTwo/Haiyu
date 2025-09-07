using System.Text.Json.Serialization;

namespace Waves.Api.Models.CloudGame;


public class CloudSendSMS
{
    [JsonPropertyName("codes")]
    public int Codes { get; set; }

    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; }
}
