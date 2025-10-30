using Haiyu.Models.ColorFullGame;
using Haiyu.Models.Enums;
using System.Text.Json.Serialization;

namespace Haiyu.Models.ColorGames;

public class ColorInfo
{

    [JsonPropertyName("gameMode")]
    public ColorGameEditMode GameMode { get; set; }

    [JsonPropertyName("cells")]
    public List<ColorCell> Cells { get; set; }

    [JsonPropertyName("setups")]
    public double Setups { get; set; }

    [JsonPropertyName("maxRows")]
    public int MaxRows { get; set; }

    [JsonPropertyName("maxColumns")]
    public int MaxColumns { get; set; }

    [JsonPropertyName("GameObjective")]
    public string GameObjective { get; set; }

    [JsonPropertyName("gamefile")]
    public string GameFile { get; set; }
}
