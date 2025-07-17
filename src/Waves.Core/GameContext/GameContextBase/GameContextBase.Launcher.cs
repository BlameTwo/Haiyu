using System.Text.Json;
using Waves.Api.Models;
using Waves.Core.GameContext.Contexts;
using Waves.Core.Models;
using Waves.Core.Models.Downloader;

namespace Waves.Core.GameContext;

partial class GameContextBase
{
    public async Task<GameLauncherSource?> GetGameLauncherSourceAsync(
        CancellationToken token = default
    )
    {
        var url = "";
        try
        {
            if (this.ContextName == nameof(GlobalGameContext))
            {
                url =
                    $"{GameAPIConfig.BaseAddress[1]}/launcher/game/{Config.GameID}/{Config.AppId}_{Config.AppKey}/index.json?_t={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            }
            else
            {
                url =
                    $"{GameAPIConfig.BaseAddress[0]}/launcher/game/{Config.GameID}/{Config.AppId}_{Config.AppKey}/index.json?_t={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            }
            var result = await HttpClientService.GameDownloadClient.GetAsync(url);
            var jsonStr = await result.Content.ReadAsStringAsync();
            var launcherIndex = JsonSerializer.Deserialize<GameLauncherSource>(
                jsonStr,
                GameLauncherSourceContext.Default.GameLauncherSource
            );
            return launcherIndex;
        }
        catch (Exception ex)
        {
            Logger.WriteError($"请求{url}出错：{ex.Message}");
            return null;
        }
    }

    public async Task<IndexGameResource> GetGameResourceAsync(
        ResourceDefault ResourceDefault,
        CancellationToken token = default
    )
    {
        var resourceIndexUrl =
            ResourceDefault.CdnList.Where(x => x.P != 0).OrderBy(x => x.P).First().Url
            + ResourceDefault.Config.IndexFile;
        var result = await HttpClientService.HttpClient.GetAsync(resourceIndexUrl, token);
        var jsonStr = await result.Content.ReadAsStringAsync();
        var launcherIndex = JsonSerializer.Deserialize<IndexGameResource>(
            jsonStr,
            IndexGameResourceContext.Default.IndexGameResource
        );
        return launcherIndex;
    }

    public async Task<PatchIndexGameResource?> GetPatchGameResourceAsync(
        string url,
        CancellationToken token = default
    )
    {
        try
        {
            var result = await HttpClientService.HttpClient.GetAsync(url, token);
            result.EnsureSuccessStatusCode();
            var jsonStr = await result.Content.ReadAsStringAsync();
            var pathIndexSource = JsonSerializer.Deserialize<PatchIndexGameResource>(
                jsonStr,
                PathIndexGameResourceContext.Default.PatchIndexGameResource
            );
            return pathIndexSource;
        }
        catch (Exception ex)
        {
            Logger.WriteError($"请求{url}出错：{ex.Message}");
            return null;
        }
    }

    public async Task<GameLauncherStarter?> GetLauncherStarterAsync(
        CancellationToken token = default
    )
    {
        string url = "";
        try
        {
            if (this.ContextName == nameof(GlobalGameContext))
            {
                url =
                    $"{GameAPIConfig.BaseAddress[1]}/launcher/{this.Config.AppId}_{this.Config.AppKey}/{this.Config.GameID}/information/{this.Config.Language}.json?_t={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            }
            else
            {
                url =
                    $"{GameAPIConfig.BaseAddress[0]}/launcher/{this.Config.AppId}_{this.Config.AppKey}/{this.Config.GameID}/information/{this.Config.Language}.json?_t={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            }
            var result = await HttpClientService.HttpClient.GetAsync(url, token);
            result.EnsureSuccessStatusCode();
            var jsonStr = await result.Content.ReadAsStringAsync();
            var pathIndexSource = JsonSerializer.Deserialize<GameLauncherStarter>(
                jsonStr,
                GameLauncherStarterContext.Default.GameLauncherStarter
            );
            return pathIndexSource;
        }
        catch (Exception ex)
        {
            Logger.WriteError($"请求{url}出错：{ex.Message}");
            return null;
        }
    }
}
