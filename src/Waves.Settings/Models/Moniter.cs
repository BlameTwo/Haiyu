using System.Text.Json.Serialization;

namespace Waves.Settings.Models;

/// <summary>
/// PC监控参数，仅针对于UI
/// </summary>
public class MoniterTheme
{
    [JsonPropertyName("fontSize")]
    public double FontSize { get; set; } = 14;

    [JsonPropertyName("fontColor")]
    public string FontColor { get; set; } = "#FFFFFF";

    [JsonPropertyName("isAllowDesktop")]
    public bool IsAllowDesktop { get; set; } = true;

    [JsonPropertyName("fontFamily")]
    public string FontFamily { get; set; } = "微软雅黑";

    [JsonPropertyName("monitorItemSpacing")]
    public double MonitorItemSpacing { get; set; } = 5;
}

public class FPSUIConfig
{
    [JsonPropertyName("currentFpsEanble")]
    public bool CurrentFpsEnable { get; set; }

    [JsonPropertyName("fps1LowEnable")]
    public bool FPS1LowEnable{ get; set; }

    [JsonPropertyName("fps01LowEnable")]
    public bool FPS01LowEnable{ get; set; }

    [JsonPropertyName("fpsMaxEnable")]
    public bool FPSMaxEnable { get; set; }

    [JsonPropertyName("maxFrameTimeEnable")]
    public bool MaxFrameTimeEnable { get; set; }
}

public class CPUUIConfig
{

    [JsonPropertyName("loadEnable")]
    public bool LoadEnable { get; set; }

    [JsonPropertyName("clockEnable")]
    public bool ClockEnable { get; set; }

    [JsonPropertyName("tempateEnable")]
    public bool TempateEnable { get; set; }

    [JsonPropertyName("voltagesEnable")]
    public bool VoltagesEnable { get; set; }
}

public class GPUUIConfig
{

    [JsonPropertyName("loadEnable")]
    public bool LoadEnable { get; set; }

    [JsonPropertyName("clockEnable")]
    public bool ClockEnable { get; set; }

    [JsonPropertyName("tempateEnable")]
    public bool TempateEnable { get; set; }

    [JsonPropertyName("voltagesEnable")]
    public bool VoltagesEnable { get; set; }

    [JsonPropertyName("vMemoryEnable")]
    public bool VMemoryEnable { get; set; }

}

/// <summary>
/// 其他单项开关
/// </summary>
public class MoniterUIItem
{
    [JsonPropertyName("enable")]
    public bool Enable { get; set; }
}


[JsonSerializable(typeof(MoniterTheme))]
[JsonSerializable(typeof(FPSUIConfig))]
[JsonSerializable(typeof(CPUUIConfig))]
[JsonSerializable(typeof(GPUUIConfig))]
[JsonSerializable(typeof(MoniterUIItem))]
public partial class MoniterJsonContext : JsonSerializerContext
{

}
