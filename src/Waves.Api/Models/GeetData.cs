using System.Text.Json;
using System.Text.Json.Serialization;

namespace Waves.Api.Models;


public class GeetData
{
    [JsonPropertyName("captcha_id")]
    public string CaptchaId { get; set; }

    [JsonPropertyName("lot_number")]
    public string LotNumber { get; set; }

    [JsonPropertyName("pass_token")]
    public string PassToken { get; set; }

    [JsonPropertyName("gen_time")]
    public string GenTime { get; set; }

    [JsonPropertyName("captcha_output")]
    public string CaptchaOutput { get; set; }
}

[JsonSerializable(typeof(GeetData))]
public partial class GeetContext:JsonSerializerContext
{

}