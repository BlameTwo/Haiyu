using System.Runtime.CompilerServices;
using System.Text.Json;
using Waves.Core.Models;

namespace Waves.Core;

public class AppSettings
{
    public static string BassFolder =>
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Waves";

    public static string RecordFolder => BassFolder + "\\RecordCache";

    public static string WrallpaperFolder => BassFolder + "\\WallpaperImages";

    public static string ScreenCaptures => BassFolder + "\\ScreenCaptures";

    public static string ColorGameFolder => BassFolder + "\\ColorGameFolder";

    public string ToolsPosionFilePath => BassFolder + "\\ToolsPostion.json";

    private static readonly string SettingsFilePath = Path.Combine(BassFolder, "System.json");
    public static readonly string LogPath = BassFolder + "\\appLogs\\appLog.log";

    public static readonly string CloudFolderPath = BassFolder + "\\Cloud";

    public const string RpcVersion = "1.0";

    // 存储所有设置的内存缓存
    private static List<LocalSettings> _settingsCache;

    static AppSettings()
    {
        LoadSettings();
    }

    private static void LoadSettings()
    {
        if (File.Exists(SettingsFilePath))
        {
            var json = File.ReadAllText(SettingsFilePath);
            try
            {
                _settingsCache = JsonSerializer.Deserialize<List<LocalSettings>>(
                    json,
                    LocalSettingsJsonContext.Default.ListLocalSettings
                );
            }
            catch (Exception)
            {
                _settingsCache = new();
            }

            SaveSettings();
        }
        else
        {
            _settingsCache = new List<LocalSettings>();
        }
    }

    private static void SaveSettings()
    {
        var json = JsonSerializer.Serialize(
            _settingsCache,
            LocalSettingsJsonContext.Default.ListLocalSettings
        );
        File.WriteAllText(SettingsFilePath, json);
    }
#nullable enable
    public static string? Token
    {
        get => Read();
        set => Write(value);
    }

    public static string? WallpaperType
    {
        get => Read();
        set => Write(value);
    }

    public static string? AreaCounterPostion
    {
        get => Read();
        set => Write(value);
    }

    public static string? TokenId
    {
        get => Read();
        set => Write(value);
    }

    public static string? AutoSignCommunity
    {
        get => Read();
        set => Write(value);
    }

    public static string? WallpaperPath
    {
        get => Read();
        set => Write(value);
    }
    public static string? CloseWindow
    {
        get => Read();
        set => Write(value);
    }

    public static string? SelectCursor
    {
        get => Read();
        set => Write(value);
    }

    public static string? CaptureModifierKey
    {
        get => Read();
        set => Write(value);
    }

    public static string? CaptureKey
    {
        get => Read();
        set => Write(value);
    }

    public static string? IsCapture
    {
        get => Read();
        set => Write(value);
    }

    public static string? Language
    {
        get => Read();
        set => Write(value);
    }

    public static string? AutoOOBE
    {
        get => Read();
        set => Write(value);
    }
    public static string ElementTheme
    {
        get => Read();
        set => Write(value);
    }


    public static string? TokenDid
    {
        get => Read();
        set => Write(value);
    }
    public static string? RpcToken
    {
        get => Read();
        set => Write(value);
    }
    public static string? WavesAutoOpenContext
    {
        get => Read();
        set => Write(value);
    }
    public static string? PunishAutoOpenContext
    {
        get => Read();
        set => Write(value);
    }


    internal static string? Read([CallerMemberName] string key = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var item = _settingsCache.FirstOrDefault(x => x.Key == key);
            return item?.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }

    internal static void Write(string? value, [CallerMemberName] string key = null)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new IOException("找不到相关Key");
        }

        if (value == null)
        {
            _settingsCache.RemoveAll(x => x.Key == key);
        }
        else
        {
            var existing = _settingsCache.FirstOrDefault(x => x.Key == key);
            if (existing != null)
            {
                existing.Value = value;
            }
            else
            {
                _settingsCache.Add(new LocalSettings { Key = key, Value = value });
            }
        }

        SaveSettings();
    }
}
