using Newtonsoft.Json;
using Windows.Graphics;

namespace WutheringWavesTool.Models;

public class ToolManagerPostion
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("rectValue")]
    public RectInt32 rectValue { get; set; }
}
