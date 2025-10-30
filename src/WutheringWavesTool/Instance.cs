using Haiyu.Contracts;
using Haiyu.Models.Dialogs;
using Haiyu.Pages.GameWikis;
using Haiyu.Pages.Record;
using Haiyu.Plugin;
using Haiyu.Services.DialogServices;
using Haiyu.Services.Navigations.NavigationViewServices;
using Haiyu.ViewModel;
using Haiyu.ViewModel.GameViewModels;
using Haiyu.ViewModel.WikiViewModels;
using Waves.Core.Services;

namespace Haiyu;

public static class Instance
{
    public static IServiceProvider Service { get; private set; }

    public static void InitService()
    {
        Service = new ServiceCollection()
            #region View and ViewModel
            .AddSingleton<ShellPage>()
            .AddSingleton<ShellViewModel>()
            .AddTransient<ToolPage>()
            .AddTransient<ToolViewModel>()
            .AddTransient<PlayerRecordPage>()
            .AddTransient<PlayerRecordViewModel>()
            .AddTransient<SettingViewModel>()
            .AddTransient<CommunityViewModel>()
            .AddTransient<GameResourceDialog>()
            .AddTransient<GameResourceViewModel>()
            .AddTransient<DeviceInfoPage>()
            .AddTransient<DeviceInfoViewModel>()
            .AddTransient<ResourceBriefViewModel>()
            .AddTransient<CloudGameViewModel>()
            .AddTransient<ColorFullGame>()
            .AddTransient<ColorFullViewModel>()
            .AddTransient<StartColorFullGamePage>()
            .AddTransient<StartColorFullGameViewModel>()
            .AddTransient<AnalysisRecordViewModel>()
            .AddTransient<AnalysisRecordPage>()
            .AddTransient<HomeViewModel>()
        #region ColorGame
        #endregion
        #region GameContext
            .AddTransient<MainGameViewModel>()
            .AddTransient<BiliBiliGameViewModel>()
            .AddTransient<GlobalGameViewModel>()
            .AddTransient<MainPGRViewModel>()
            .AddTransient<TwPGRGameViewModel>()
            .AddTransient<GlobalPGRViewModel>()
            .AddTransient<BiliBiliPGRGameViewModel>()
        #endregion
        #region Wiki
            .AddTransient<WavesWikiViewModel>()
        #endregion
        #region Community
            .AddTransient<GamerSignPage>()
            .AddTransient<GamerSignViewModel>()
            .AddTransient<GamerRoilsDetilyViewModel>()
            .AddTransient<GameRoilsViewModel>()
            .AddTransient<GamerDockViewModel>()
            .AddTransient<GamerChallengeViewModel>()
            .AddTransient<GamerExploreIndexViewModel>()
            .AddTransient<GamerTowerViewModel>()
            .AddTransient<GamerSkinViewModel>()
            .AddTransient<GamerSlashDetailViewModel>()
            #endregion
            #region Record
            .AddTransient<RecordItemViewModel>()
            #endregion
            #region Roil
            .AddTransient<GamerRoilsDetilyPage>()
            .AddTransient<GamerRoilViewModel>()
            #endregion
            #region Dialog
            .AddTransient<LoginDialog>()
            .AddTransient<LoginGameViewModel>()
            .AddTransient<GameLauncherCacheManager>()
            .AddTransient<GameLauncherCacheViewModel>()
            .AddTransient<WebGameLogin>()
            .AddTransient<WebGameViewModel>()
            .AddTransient<BindGameDataDialog>()
            .AddTransient<BindGameDataViewModel>()
            .AddTransient<SelectGameFolderDialog>()
            .AddTransient<SelectGameFolderViewModel>()
            .AddTransient<CloseDialog>()
            .AddTransient<SelectDownoadGameDialog>()
            .AddTransient<SelectDownloadGameViewModel>()
            .AddTransient<QRLoginDialog>()
            .AddTransient<QrLoginViewModel>()
            #endregion
            #endregion
            #region Navigation
            .AddTransient<IPageService, PageService>()
            .AddTransient<IPickersService, PickersService>()
            .AddSingleton<ITipShow, TipShow>()
            .AddKeyedTransient<ITipShow,PageTipShow>("Cache")
            .AddKeyedTransient<IDialogManager,MainDialogService>("Cache")
            .AddTransient<IColorGameManager,ColorGameManager>()
        #endregion
        #region Base
            .AddSingleton<IAppContext<App>, AppContext<App>>()
            .AddSingleton<IWavesClient, WavesClient>()
            .AddSingleton<ICloudGameService, CloudGameService>()
            .AddSingleton<IScreenCaptureService,ScreenCaptureService>()
            .AddSingleton<IGameWikiClient,GameWikiClient>()
            .AddTransient<IViewFactorys, ViewFactorys>()
            .AddSingleton<CloudConfigManager>(
                (s) =>
                {
                    var mananger = new CloudConfigManager(AppSettings.CloudFolderPath);
                    return mananger;
                }
            )
            .AddSingleton<IWallpaperService, WallpaperService>(
                (s) =>
                {
                    var service = new WallpaperService(s.GetRequiredService<ITipShow>());
                    service.RegisterHostPath(App.WrallpaperFolder);
                    return service;
                }
            )
            #endregion
            #region Navigation
            .AddKeyedSingleton<INavigationService, HomeNavigationService>(
                nameof(HomeNavigationService)
            )
            .AddKeyedSingleton<INavigationViewService, HomeNavigationViewService>(
                nameof(HomeNavigationViewService)
            )
            .AddKeyedTransient<INavigationService, CommunityNavigationService>(
                nameof(CommunityNavigationService)
            )
            .AddKeyedTransient<INavigationService, WebGameNavigationService>(
                nameof(WebGameNavigationService)
            ).AddKeyedTransient<INavigationService, GameWikiNavigationService>(
                nameof(GameWikiNavigationService)
            )
            #endregion
            #region Plugin
            
            #endregion
            .AddKeyedSingleton<IDialogManager, MainDialogService>(nameof(MainDialogService))
            .AddKeyedSingleton<LoggerService>(
                "AppLog",
                (s, e) =>
                {
                    var logger = new LoggerService();
                    logger.InitLogger(AppSettings.LogPath, Serilog.RollingInterval.Day);
                    return logger;
                }
            )
            #region Record
            .AddScoped<IDialogManager, ScopeDialogService>()
            .AddScoped<ITipShow, TipShow>()
            .AddKeyedScoped<IPlayerRecordContext, PlayerRecordContext>("PlayerRecord")
            .AddKeyedScoped<INavigationService, RecordNavigationService>(
                nameof(RecordNavigationService)
            )
            .AddScoped<IRecordCacheService, RecordCacheService>()
            .AddKeyedScoped<IGamerRoilContext, GamerRoilContext>(nameof(GamerRoilContext))
            .AddKeyedScoped<INavigationService, GameRoilNavigationService>(
                nameof(GameRoilNavigationService)
            )
            #endregion
            .AddGameContext()
            .BuildServiceProvider();
    }

    public static T? GetService<T>()
        where T : notnull
    {
        if (Service.GetRequiredService<T>() is not T v)
        {
            throw new ArgumentException("服务未注入");
            ;
        }
        return v;
    }
}
