using Waves.Core.Services;

namespace Haiyu.Services;

public sealed class LauncherTaskService : ILauncherTaskService
{
    public LauncherTaskService(
        ITipShow tipShow,
        [FromKeyedServices("AppLog")] LoggerService loggerService,
        IKuroClient wavesClient
    )
    {
        TipShow = tipShow;
        LoggerService = loggerService;
        WavesClient = wavesClient;
    }

    public ITipShow TipShow { get; }
    public LoggerService LoggerService { get; }
    public IKuroClient WavesClient { get; }

    public async Task RunAsync(CancellationToken token)
    {
        try
        {
            if (Boolean.TryParse(AppSettings.AutoSignCommunity, out var flag) && flag)
            {
                int signErrorCount = 0;
                var gamers = await WavesClient.GetWavesGamerAsync(token);
                if (gamers == null || gamers.Success == false)
                {
                    return;
                }
                foreach (var item in gamers.Data)
                {
                    var result = await WavesClient.SignInAsync(
                        item.UserId.ToString(),
                        item.RoleId,
                        token
                    );
                    if (result is null || (!result.Success && result.Code != 1511))
                    {
                        signErrorCount++;
                    }
                }
                await TipShow.ShowMessageAsync(
                    $"自动签到结果：{gamers.Data.Count - signErrorCount}个签到成功",
                    Symbol.Message
                );
            }
        }
        catch (Exception ex)
        {
            LoggerService.WriteError(ex.Message);
        }
    }
}
