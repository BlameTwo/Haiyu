using Waves.Settings.Models;

namespace Waves.Settings;

#region CPU

[Settings<MoniterTheme>(
    Name = "MoniterTheme",
    Nullable = true,
    JsonTypeInfoContextType = typeof(MoniterJsonContext),
    JsonTypeInfoPropertyName = nameof(MoniterJsonContext.Default.MoniterTheme)
)]
[Settings<FPSUIConfig>(
    Name = "MoniterFPS",
    Nullable = true,
    JsonTypeInfoContextType = typeof(MoniterJsonContext),
    JsonTypeInfoPropertyName = nameof(MoniterJsonContext.Default.FPSUIConfig)
)]
[Settings<CPUUIConfig>(
    Name = "MoniterCPU",
    Nullable = true,
    JsonTypeInfoContextType = typeof(MoniterJsonContext),
    JsonTypeInfoPropertyName = nameof(MoniterJsonContext.Default.CPUUIConfig)
)]
[Settings<GPUUIConfig>(
    Name = "MoniterGPU",
    Nullable = true,
    JsonTypeInfoContextType = typeof(MoniterJsonContext),
    JsonTypeInfoPropertyName = nameof(MoniterJsonContext.Default.GPUUIConfig)
)]
[Settings<MoniterUIItem>(
    Name = "MoniterPower",
    Nullable = true,
    JsonTypeInfoContextType = typeof(MoniterJsonContext),
    JsonTypeInfoPropertyName = nameof(MoniterJsonContext.Default.MoniterUIItem)
)]
[Settings<MoniterUIItem>(
    Name = "MoniterMemory",
    Nullable = true,
    JsonTypeInfoContextType = typeof(MoniterJsonContext),
    JsonTypeInfoPropertyName = nameof(MoniterJsonContext.Default.MoniterUIItem)
)]
#endregion
public sealed partial class MoniterSettings : SettingBase
{
    public MoniterSettings(string configPath)
        : base(configPath) { }
}
