using System.Text.Json.Serialization;

namespace Waves.Api.Models;

public class AddUserDatum
{
    [JsonPropertyName("serverId")]
    public string ServerId { get; set; }

    [JsonPropertyName("serverName")]
    public string ServerName { get; set; }
}

public class AddUserGameServer
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("data")]
    public List<AddUserDatum> Data { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
public class SendGameVerifyCode
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
public class BindGameVerifyCode
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public TokenData Data { get; set; }
}

public class TokenData
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}
