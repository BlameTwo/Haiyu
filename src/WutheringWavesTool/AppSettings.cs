

namespace WutheringWavesTool;

public class AppSettings
{
    private static readonly string SettingsFilePath = Path.Combine(App.BassFolder, "System.json");

    public static readonly string CloudFolderPath = App.BassFolder+"\\Cloud";

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

    public static string? Token
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
            Debug.WriteLine("Attempted to write setting with a null or empty key.");
            return;
        }

        if (value == null)
        {
            // 移除指定 key 的项
            _settingsCache.RemoveAll(x => x.Key == key);
        }
        else
        {
            // 查找是否存在该 key
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
