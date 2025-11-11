using Microsoft.Extensions.DependencyInjection;
using Waves.Core.Contracts;
using Waves.Core.GameContext;
using Waves.Core.GameContext.Contexts;
using Waves.Core.GameContext.Contexts.PRG;
using Waves.Core.Services;

namespace Waves.Core;

public static class Waves
{
    /// <summary>
    /// 注入游戏上下文，注意已包含HttpClientFactory
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGameContext(this IServiceCollection services)
    {
        services
            .AddTransient<IHttpClientService, HttpClientService>()
            .AddKeyedSingleton<IGameContext, MainGameContext>(
                nameof(MainGameContext),
                (provider, c) =>
                {
                    var context = GameContextFactory.GetMainGameContext();
                    context.HttpClientService = provider.GetRequiredService<IHttpClientService>();
                    return context;
                }
            )
            .AddKeyedSingleton<IGameContext, GlobalGameContext>(
                nameof(GlobalGameContext),
                (provider, c) =>
                {
                    var context = GameContextFactory.GetGlobalGameContext();
                    context.HttpClientService = provider.GetRequiredService<IHttpClientService>();
                    return context;
                }
            )
            .AddKeyedSingleton<IGameContext, BiliBiliGameContext>(
                nameof(BiliBiliGameContext),
                (provider, c) =>
                {
                    var context = GameContextFactory.GetBilibiliGameContext();
                    context.HttpClientService = provider.GetRequiredService<IHttpClientService>();
                    return context;
                }
            )
            .AddKeyedSingleton<IGameContext, MainPGRGameContext>(
                nameof(MainPGRGameContext),
                (provider, c) =>
                {
                    var context = GameContextFactory.GetMainPGRGameContext();
                    context.HttpClientService = provider.GetRequiredService<IHttpClientService>();
                    return context;
                }
            )
            .AddKeyedSingleton<IGameContext, BiliBiliPRGGameContext>(
                nameof(BiliBiliPRGGameContext),
                (provider, c) =>
                {
                    var context = GameContextFactory.GetBiliBiliPRGGameContext();
                    context.HttpClientService = provider.GetRequiredService<IHttpClientService>();
                    return context;
                }
            )
            .AddKeyedSingleton<IGameContext, GlobalPRGGameContext>(
                nameof(GlobalPRGGameContext),
                (provider, c) =>
                {
                    var context = GameContextFactory.GetGlobalPGRGameContext();
                    context.HttpClientService = provider.GetRequiredService<IHttpClientService>();
                    return context;
                }
            )
            .AddKeyedSingleton<IGameContext, TwPGRGameContext>(
                nameof(TwPGRGameContext),
                (provider, c) =>
                {
                    var context = GameContextFactory.GetTwWavesGameContext();
                    context.HttpClientService = provider.GetRequiredService<IHttpClientService>();
                    return context;
                }
            )
            
            .AddTransient<IHttpClientService, HttpClientService>();
        return services;
    }
}
