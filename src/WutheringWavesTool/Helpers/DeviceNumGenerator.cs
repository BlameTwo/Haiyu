using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Haiyu.Helpers;

internal class DeviceNumGenerator
{
    private static Dictionary<string, string> localStorage = new Dictionary<string, string>();
    private const string UUID_KEY_NAME = "__KrSDK_UUID__";
    private const string CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static Random random = new Random();

    /// <summary>
    /// 解析环境变量并获取包含deviceNum的请求基础参数
    /// </summary>
    /// <param name="env">环境变量字典</param>
    /// <returns>请求基础参数</returns>
    public static Dictionary<string, string> ParserEnv(Dictionary<string, string> env)
    {
        var result = new Dictionary<string, string>();

        // 处理以VITE_APP_BASEHTTPREQ开头的环境变量
        foreach (var key in env.Keys.Where(k => k.StartsWith("VITE_APP_BASEHTTPREQ")))
        {
            var newKey = key.Replace("VITE_APP_BASEHTTPREQ_", "");
            result[newKey] = env[key];
        }

        // 添加deviceNum及其他必要参数
        result["deviceNum"] = Uuid(32);
        result["version"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        result["sdkVersion"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        result["response_type"] = "code";

        return result;
    }

    /// <summary>
    /// 获取或创建UUID
    /// </summary>
    /// <param name="length">UUID长度</param>
    /// <param name="charSetLength">字符集长度</param>
    /// <returns>UUID字符串</returns>
    public static string Uuid(int length, int charSetLength = -1)
    {
        // 尝试从存储中获取已有的UUID
        if (localStorage.ContainsKey(UUID_KEY_NAME))
        {
            return localStorage[UUID_KEY_NAME];
        }

        // 创建新的UUID
        string newUuid = CreateUuid(length, charSetLength);

        // 保存到存储
        localStorage[UUID_KEY_NAME] = newUuid;

        return newUuid;
    }

    /// <summary>
    /// 创建指定长度的随机UUID
    /// </summary>
    /// <param name="length">UUID长度</param>
    /// <param name="charSetLength">字符集长度</param>
    /// <returns>生成的UUID字符串</returns>
    public static string CreateUuid(int length, int charSetLength)
    {
        charSetLength = charSetLength > 0 ? charSetLength : CHARS.Length;
        var result = new char[length];

        if (length > 0)
        {
            for (int i = 0; i < length; i++)
            {
                result[i] = CHARS[random.Next(charSetLength)];
            }
        }

        return new string(result);
    }

    /// <summary>
    /// 获取HTTP请求的基础参数
    /// </summary>
    /// <param name="env">环境变量</param>
    /// <returns>包含基础参数的字典</returns>
    public static Dictionary<string, string> GetHttpBaseReq(Dictionary<string, string> env)
    {
        var baseParams = new Dictionary<string, string>
    {
        { "redirect_uri", "1" },
        { "__e__", "1" },
        { "pack_mark", "1" }
    };

        var parsedEnv = ParserEnv(env);

        // 合并基础参数和解析的环境参数
        foreach (var kvp in parsedEnv)
        {
            baseParams[kvp.Key] = kvp.Value;
        }

        return baseParams;
    }

    private static readonly Random _random = new Random();
    private static long _sequence = 0;
    private static readonly object _lock = new object();

    /// <summary>
    /// 生成类似 "198790753a2bfb-06d53a53a717f88-4c657b58-1327104-198790753a31598" 的ID
    /// </summary>
    public static string GenerateId()
    {
        // 1. 机器码/随机前缀 (14 hex)
        string part1 = GetRandomHex(14);

        // 2. 时间戳 + 随机 (15 hex)
        long ticks = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        string part2 = (ticks ^ _random.Next()).ToString("x");

        // 3. 时间戳低位 (8 hex)
        int timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string part3 = timestamp.ToString("x8");

        // 4. 自增序列 (7 digits)
        long seq;
        lock (_lock)
        {
            _sequence = (_sequence + 1) % 10_000_000; // 保证在 7 位以内
            seq = _sequence;
        }
        string part4 = seq.ToString("D7");

        // 5. 随机后缀 (15 hex)
        string part5 = GetRandomHex(15);

        return $"{part1}-{part2}-{part3}-{part4}-{part5}";
    }

    /// <summary>
    /// 获取指定位数的十六进制字符串
    /// </summary>
    private static string GetRandomHex(int length)
    {
        byte[] buffer = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
        }

        StringBuilder sb = new StringBuilder(length);
        foreach (byte b in buffer)
        {
            sb.Append(b.ToString("x2"));
            if (sb.Length >= length)
                break;
        }
        return sb.ToString(0, length);
    }
}


