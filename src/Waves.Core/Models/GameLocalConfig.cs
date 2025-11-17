using MemoryPack;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Waves.Core.Models;

public class GameLocalSettingName
{
    /// <summary>
    /// 游戏启动文件夹
    /// </summary>
    public const string GameLauncherBassFolder = nameof(GameLauncherBassFolder);

    /// <summary>
    /// 游戏启动可执行文件
    /// </summary>
    public const string GameLauncherBassProgram = nameof(GameLauncherBassProgram);

    /// <summary>
    /// 本地游戏版本
    /// </summary>
    public const string LocalGameVersion = nameof(LocalGameVersion);

    public const string LocalGameUpdateing = nameof(LocalGameUpdateing);

    /// <summary>
    /// 下载速度
    /// </summary>
    public const string LimitSpeed = nameof(LimitSpeed);

    /// <summary>
    /// 是否使用DX11启动
    /// </summary>
    public const string IsDx11 = nameof(IsDx11);

    public const string GameRunTotalTime = nameof(GameRunTotalTime);

    /// <summary>
    /// 预下载路径
    /// </summary>
    public const string ProdDownloadFolderPath = nameof(ProdDownloadFolderPath);
    public const string ProdDownloadFolderDone = nameof(ProdDownloadFolderDone);

    public const string ProdDownloadVersion = nameof(ProdDownloadVersion);

    public const string GameTime = nameof(GameTime);
}


public class GameLocalConfig
{
    private Dictionary<string, string> _settings = new Dictionary<string, string>();
    private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

    // 创建一个JsonSerializerContext实例，它包含了为AOT预生成的代码
    private readonly LocalSettingsJsonContext _jsonContext = new LocalSettingsJsonContext(
        new JsonSerializerOptions
        {
            WriteIndented = true // 让输出的JSON更易读
        }
    );

    public string SettingPath { get; set; }

    public GameLocalConfig(string settingPath)
    {
        SettingPath = settingPath;
        LoadConfig();
    }

    /// <summary>
    /// 从JSON文件加载配置到内存
    /// </summary>
    private void LoadConfig()
    {
        if (!File.Exists(SettingPath))
        {
            _settings = new Dictionary<string, string>();
            return;
        }

        try
        {
            var jsonString = File.ReadAllText(SettingPath);

            // 使用我们AOT友好的上下文进行反序列化
            var loadedSettings = JsonSerializer.Deserialize(jsonString, typeof(Dictionary<string, string>), _jsonContext) as Dictionary<string, string>;
            _settings = loadedSettings ?? new Dictionary<string, string>();
        }
        catch
        {
            _settings = new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// 将内存中的配置异步保存到JSON文件
    /// </summary>
    private async Task SaveConfigToFileAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var jsonString = JsonSerializer.Serialize(_settings, typeof(Dictionary<string, string>), _jsonContext);
            await File.WriteAllTextAsync(SettingPath, jsonString);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<bool> SaveConfigAsync(string key, string value)
    {
        try
        {
            _settings[key] = value;
            await SaveConfigToFileAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetConfig(string key)
    {
        if (_settings.TryGetValue(key, out string value))
        {
            return value;
        }

        return null;
    }
}

public class LocalSettings
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}



[JsonSerializable(typeof(LocalSettings))]
[JsonSerializable(typeof(List<LocalSettings>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class LocalSettingsJsonContext : JsonSerializerContext { };
