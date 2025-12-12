using Waves.Api.Models.CloudGame;

namespace Haiyu.Services;

public sealed partial class CloudConfigManager
{
    public string Path { get; }

    public CloudConfigManager(string savePath)
    {
        this.Path = savePath;
    }

    public Dictionary<string, LoginData> cacheData;



    public async Task<ObservableCollection<LoginData>> GetUsersAsync(CancellationToken token = default)
    {
        ObservableCollection<LoginData> logins = new ObservableCollection<LoginData>();
        foreach (var item in Directory.GetFiles(this.Path, "*.json"))
        {
            try
            {
                var result = JsonSerializer.Deserialize(
                    await File.ReadAllTextAsync(item, token),
                    CloundContext.Default.LoginData
                );
                logins.Add(result);
            }
            catch (Exception)
            {
                continue;
            }
        }
        foreach (var item in logins)
        {
            cacheData = logins.ToDictionary(x => x.Username, x => x);
        }
        return logins;
    }

    public async Task<LoginData?> GetUserAsync(string userName,CancellationToken token = default)
    {
        List<LoginData> items = null;
        if(cacheData == null || cacheData.Count == 0)
            items = (await GetUsersAsync()).ToList();
        else
            items = cacheData.Values.ToList();
        return items.Where(x => x.Username == userName).FirstOrDefault();
    }

    public async Task<bool> SaveUserAsync(LoginData loginResult)
    {
        try
        {
            foreach (var item in Directory.GetFiles(this.Path, ".json"))
            {
                var result = JsonSerializer.Deserialize(
                    await File.ReadAllTextAsync(item),
                    CloundContext.Default.LoginData
                );
                if (result.Username == loginResult.Username)
                {
                    File.Delete(item);
                }
            }
            await File.WriteAllTextAsync(
                Path + $"\\{loginResult.Username}.json",
                JsonSerializer.Serialize(loginResult, CloundContext.Default.LoginData)
            );
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
