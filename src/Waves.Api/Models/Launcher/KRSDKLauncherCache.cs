using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Waves.Api.Models.Messanger;

namespace Waves.Api.Models.Launcher;

public class KRSDKLauncherCache
{
    [JsonPropertyName("cuid")]
    public string Cuid { get; set; }

    [JsonPropertyName("id")]
    public double Id { get; set; }

    [JsonPropertyName("loginType")]
    public int LoginType { get; set; }

    [JsonPropertyName("oauthCode")]
    public string OauthCode { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("thirdNickName")]
    public string ThirdNickName { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonIgnore]
    public IRelayCommand CopyOAuthCodeCommand =>
        new RelayCommand(() =>
            WeakReferenceMessenger.Default.Send(new GameLauncheCacheMessager(this, true))
        );
}
