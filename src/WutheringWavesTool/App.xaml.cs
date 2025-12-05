using System.Globalization;
using Haiyu.Helpers;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.Windows.AppLifecycle;
using Waves.Core.Services;
using Windows.Globalization;

namespace Haiyu;

public partial class App : ClientApplication
{
    public static string BassFolder =>
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Waves";

    public static string RecordFolder => BassFolder + "\\RecordCache";

    public static string WrallpaperFolder => BassFolder + "\\WallpaperImages";

    public static string ScreenCaptures => BassFolder + "\\ScreenCaptures";

    public static string ColorGameFolder => BassFolder + "\\ColorGameFolder";

    public string ToolsPosionFilePath => App.BassFolder + "\\ToolsPostion.json";

    [DllImport("shcore.dll", SetLastError = true)]
    private static extern int SetProcessDpiAwareness(int dpiAwareness);

    private const int PROCESS_PER_MONITOR_DPI_AWARE = 2;
    private AppInstance mainInstance;

    public App()
    {
        this.UnhandledException += App_UnhandledException;
        Directory.CreateDirectory(BassFolder);
        Directory.CreateDirectory(RecordFolder);
        Directory.CreateDirectory(ColorGameFolder);
        Directory.CreateDirectory(WrallpaperFolder);
        Directory.CreateDirectory(ScreenCaptures);
        Directory.CreateDirectory(Path.GetDirectoryName(AppSettings.LogPath));
        Directory.CreateDirectory(AppSettings.CloudFolderPath);
        if (AppSettings.WallpaperType == null)
        {
            AppSettings.WallpaperType = "Video";
        }
        mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey(
            "Haiyu_Main"
        );
        mainInstance.Activated += MainInstance_Activated;
        #region PE DPI Resource
        SetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE);
        #endregion
        GameContextFactory.GameBassPath = BassFolder;
        Instance.InitService();
        this.InitializeComponent();
    }

    private void MainInstance_Activated(object sender, AppActivationArguments e)
    {
        if (e.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.File) { }
    }

    private void App_UnhandledException(
        object sender,
        Microsoft.UI.Xaml.UnhandledExceptionEventArgs e
    )
    {
        try
        {
            Instance.Service.GetService<ITipShow>().ShowMessage(e.Message, Symbol.Clear);
            Instance.Service.GetKeyedService<LoggerService>("AppLog").WriteError(e.Message);
        }
        catch (Exception ex)
        {
            Instance.Service.GetKeyedService<LoggerService>("AppLog").WriteError(ex.Message);
        }
        finally
        {
            e.Handled = true;
        }
    }

    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        Instance.Service.GetKeyedService<LoggerService>("AppLog").WriteInfo("启动程序中……");
        if (string.IsNullOrWhiteSpace(LanguageService.GetLanguage()))
        {
            LanguageService.SetLanguage("zh-Hans");
            ApplicationLanguages.PrimaryLanguageOverride = "zh-Hans";
        }
        else
        {
            ApplicationLanguages.PrimaryLanguageOverride = LanguageService.GetLanguage();
        }
        var mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey(
            "Haiyu_Main"
        );
        if (!mainInstance.IsCurrent)
        {
            mainInstance.Activated -= MainInstance_Activated;
            var activatedEventArgs = Microsoft
                .Windows.AppLifecycle.AppInstance.GetCurrent()
                .GetActivatedEventArgs();
            await mainInstance.RedirectActivationToAsync(activatedEventArgs);
            Process.GetCurrentProcess().Kill();
            return;
        }
        await LanguageService.InitAsync();
        await Instance.Service.GetRequiredService<IAppContext<App>>().LauncherAsync(this);
        Instance.Service.GetService<IScreenCaptureService>()!.Register();
    }

    private void SetTheme()
    {
        var theme = Instance.Service.GetRequiredService<IThemeService>();
        switch (AppSettings.ElementTheme)
        {
            case "Light":
                theme.SetTheme(ElementTheme.Light);
                break;
            case "Dark":
                theme.SetTheme(ElementTheme.Dark);
                break;
            case "Default":
                theme.SetTheme(ElementTheme.Default);
                break;
            case null:
            default:
                theme.SetTheme(ElementTheme.Default);
                break;
        }
    }
}
