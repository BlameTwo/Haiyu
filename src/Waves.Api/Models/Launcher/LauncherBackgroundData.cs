using System.Text.Json.Serialization;

namespace Waves.Api.Models.Launcher;

public class LauncherBackgroundData
{
    [JsonPropertyName("functionSwitch")]
    public int FunctionSwitch { get; set; }

    [JsonPropertyName("backgroundFile")]
    public string BackgroundFile { get; set; }

    [JsonPropertyName("backgroundFileType")]
    public int BackgroundFileType { get; set; }

    [JsonPropertyName("firstFrameImage")]
    public string FirstFrameImage { get; set; }

    [JsonPropertyName("slogan")]
    public string Slogan { get; set; }
}
