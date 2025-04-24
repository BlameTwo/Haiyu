﻿using Waves.Core.Contracts;
using Waves.Core.GameContext.Contexts;
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
}
