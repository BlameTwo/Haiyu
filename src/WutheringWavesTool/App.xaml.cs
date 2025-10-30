

using Haiyu.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System.Text;
using Waves.Core.Services;
using Windows.ApplicationModel.Activation;

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
        mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey("Haiyu_Main");
        mainInstance.Activated += MainInstance_Activated;
        #region PE DPI Resource
        //SetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE);
        #endregion
        this.UnhandledException += App_UnhandledException;
        Directory.CreateDirectory(BassFolder);
        Directory.CreateDirectory(RecordFolder);
        Directory.CreateDirectory(ColorGameFolder);
        Directory.CreateDirectory(WrallpaperFolder);
        Directory.CreateDirectory(ScreenCaptures);
        Directory.CreateDirectory(Path.GetDirectoryName(AppSettings.LogPath));
        Directory.CreateDirectory(AppSettings.CloudFolderPath);
        GameContextFactory.GameBassPath = BassFolder;
        Instance.InitService();
        this.InitializeComponent();
        //var result2 = KRHelper.Xor(Convert.FromBase64String(File.ReadAllText(@"C:\Program Files\Punishing Gray Raven\2.2.2.0\Assets\KRApp.conf")), 99);
        //var str = Encoding.UTF8.GetString(result2);
    }

    private void MainInstance_Activated(object sender, AppActivationArguments e)
    {
        if (e.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.File)
        {
        }
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
        var mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey("Haiyu_Main");
        if (!mainInstance.IsCurrent)
        {
            mainInstance.Activated -= MainInstance_Activated;
            var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();
            await mainInstance.RedirectActivationToAsync(activatedEventArgs);
            Process.GetCurrentProcess().Kill();
            return;
        }
        
        await Instance.Service.GetRequiredService<IAppContext<App>>().LauncherAsync(this);
        Instance.Service.GetService<IScreenCaptureService>().Register();
        Instance.Service.GetKeyedService<LoggerService>("AppLog").WriteInfo("启动程序中……");
    }
}
