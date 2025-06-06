using System.Text.Json.Serialization;

namespace Waves.Api.Models;

public class RefreshToken
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
}

[JsonSerializable(typeof(RefreshToken))]
public partial class AccessTokenContext : JsonSerializerContext { }
