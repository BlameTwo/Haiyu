using Waves.Core.Contracts;
using Waves.Core.GameContext.Contexts;
using Waves.Core.GameContext.Contexts.PRG;
using Waves.Core.Models;
using Waves.Core.Services;

namespace Waves.Core.GameContext;

public static class GameContextFactory
{
    public static string GameBassPath { get; set; }

    /// <summary>
    /// 获得全部支持的游戏上下文名称
    /// </summary>
    /// <returns></returns>
    public static IReadOnlyCollection<String> GameAllContext() =>
        [
            nameof(WavesMainGameContext),
            nameof(WavesBiliBiliGameContext),
            nameof(WavesGlobalGameContext),
            nameof(PunishMainGameContext),
            nameof(PunishBiliBiliGameContext),
            nameof(PunishGlobalGameContext),
            nameof(PunishTwGameContext),
        ];

    internal static WavesBiliBiliGameContext GetBilibiliGameContext() =>
        new WavesBiliBiliGameContext(GameAPIConfig.BilibiliConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\BiliBiliConfig",
            IsLimitSpeed = false,
        };

    internal static WavesGlobalGameContext GetGlobalGameContext() =>
        new WavesGlobalGameContext(GameAPIConfig.GlobalConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\GlobalConfig",
            IsLimitSpeed = false,
        };

    internal static WavesMainGameContext GetMainGameContext() =>
        new WavesMainGameContext(GameAPIConfig.MainAPiConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\MainConfig",
            IsLimitSpeed = false,
        };

    internal static PunishMainGameContext GetMainPGRGameContext() =>
        new PunishMainGameContext(GameAPIConfig.MainBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\MainPGRConfig",
            IsLimitSpeed = false,
        };

    internal static PunishBiliBiliGameContext GetBiliBiliPRGGameContext() =>
        new PunishBiliBiliGameContext(GameAPIConfig.BiliBiliBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\BilibiliPRGConfig",
            IsLimitSpeed = false,
        };

    internal static PunishGlobalGameContext GetGlobalPGRGameContext() =>
        new PunishGlobalGameContext(GameAPIConfig.GlobalBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\GlokbalPGRConfig",
            IsLimitSpeed = false,
        };

    internal static PunishTwGameContext GetTwWavesGameContext() =>
        new PunishTwGameContext(GameAPIConfig.TWBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\TwPGRConfig",
            IsLimitSpeed = false,
        };
}
