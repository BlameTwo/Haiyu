using Waves.Core.Contracts;
using Waves.Core.GameContext.Contexts;
using Waves.Core.GameContext.Contexts.PRG;
using Waves.Core.Models;
using Waves.Core.Services;

namespace Waves.Core.GameContext;

public static class GameContextFactory
{
    public static string GameBassPath { get; set; }

    internal static BiliBiliGameContext GetBilibiliGameContext() =>
        new BiliBiliGameContext(GameAPIConfig.BilibiliConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\BiliBiliConfig",
            IsLimitSpeed = false,
        };

    internal static GlobalGameContext GetGlobalGameContext() =>
        new GlobalGameContext(GameAPIConfig.GlobalConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\GlobalConfig",
            IsLimitSpeed = false,
        };

    internal static MainGameContext GetMainGameContext() =>
        new MainGameContext(GameAPIConfig.MainAPiConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\MainConfig",
            IsLimitSpeed = false,
        };

    internal static MainPGRGameContext GetMainPGRGameContext() =>
        new MainPGRGameContext(GameAPIConfig.MainBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\MainPGRConfig",
            IsLimitSpeed = false,
        };

    internal static BiliBiliPRGGameContext GetBiliBiliPRGGameContext() =>
        new BiliBiliPRGGameContext(GameAPIConfig.BiliBiliBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\BilibiliPRGConfig",
            IsLimitSpeed = false,
        }; internal static GlobalPRGGameContext GetGlobalPGRGameContext() =>
        new GlobalPRGGameContext(GameAPIConfig.GlobalBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\GlokbalPGRConfig",
            IsLimitSpeed = false,
        };

    internal static TwPGRGameContext GetTwWavesGameContext()=>
        new TwPGRGameContext(GameAPIConfig.TWBGRConfig)
        {
            GamerConfigPath = GameContextFactory.GameBassPath + "\\TwPGRConfig",
            IsLimitSpeed = false,
        };
}
