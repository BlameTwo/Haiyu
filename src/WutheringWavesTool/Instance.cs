using Haiyu.Models.Dialogs;
using Haiyu.Services.DialogServices;
using Haiyu.Services.Navigations.NavigationViewServices;
using Haiyu.ViewModel.GameViewModels;
using Waves.Core.Services;
using Windows.Gaming.Input;
using WutheringWavesTool.Pages.Record;
using WutheringWavesTool.ViewModel;

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
            .AddTransient<AnalysisRecordViewModel>()
            .AddTransient<AnalysisRecordPage>()
        #region GameContext
            .AddTransient<MainGameViewModel>()
            .AddTransient<BiliBiliGameViewModel>()
            .AddTransient<GlobalGameViewModel>()
            .AddTransient<MainPGRViewModel>()
            .AddTransient<GlobalPGRViewModel>()
            .AddTransient<BiliBiliPGRGameViewModel>()
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
        #endregion
        #region Base
            .AddSingleton<IAppContext<App>, AppContext<App>>()
            .AddSingleton<IWavesClient, WavesClient>()
            .AddSingleton<ICloudGameService, CloudGameService>()
            .AddTransient<IViewFactorys, ViewFactorys>()
            .AddSingleton<CloudConfigManager>((s) =>
            {
                var mananger = new CloudConfigManager(AppSettings.CloudFolderPath);
                return mananger;
            })
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
            )
        #endregion
            .AddKeyedSingleton<IDialogManager, MainDialogService>(nameof(MainDialogService))
            .AddKeyedSingleton<LoggerService>("AppLog", (s,e) =>
            {
                var logger = new LoggerService();
                logger.InitLogger(AppSettings.LogPath, Serilog.RollingInterval.Day);
                return logger;
            })
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
