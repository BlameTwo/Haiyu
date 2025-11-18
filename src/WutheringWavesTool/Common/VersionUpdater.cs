using System.Net.Http.Json;

namespace Haiyu.Common;

public static class VersionUpdater
{
    public static async Task<UpdateInfo?> GetLasterPackageAsync(CancellationToken toekn)
    {
        try
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36 Edg/140.0.0.0");
            var model = await client.GetFromJsonAsync("https://api.github.com/repos/BlameTwo/Haiyu/releases/latest", UpdateInfoContext.Default.UpdateInfo, toekn);
            return model;
        }
        catch (Exception)
        {
            return null;
        }
    }
}

