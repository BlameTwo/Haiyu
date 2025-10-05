

using Waves.Core.Services;
using Haiyu.Helpers;
using System.Text;

namespace Haiyu;

public partial class App : ClientApplication
{
    public static string BassFolder =>
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Waves";

    public static string RecordFolder => BassFolder + "\\RecordCache";

    public static string WrallpaperFolder => BassFolder + "\\WallpaperImages";

    public static string ScreenCaptures => BassFolder + "\\ScreenCaptures";

    public string ToolsPosionFilePath => App.BassFolder + "\\ToolsPostion.json";
    [DllImport("shcore.dll", SetLastError = true)]
    private static extern int SetProcessDpiAwareness(int dpiAwareness);

    private const int PROCESS_PER_MONITOR_DPI_AWARE = 2;
    public App()
    {
        #region PE DPI Resource
        //SetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE);
        #endregion
        var result = HardwareIdGenerator.GenerateUniqueId();
        this.UnhandledException += App_UnhandledException;
        Directory.CreateDirectory(BassFolder);
        Directory.CreateDirectory(RecordFolder);
        Directory.CreateDirectory(WrallpaperFolder);
        Directory.CreateDirectory(ScreenCaptures);
        GameContextFactory.GameBassPath = BassFolder;
        Instance.InitService();
        this.InitializeComponent();
        var id = DeviceNumGenerator.GenerateId();
        Directory.CreateDirectory(AppSettings.CloudFolderPath);
        //var result2 = KRHelper.Xor(Convert.FromBase64String(File.ReadAllText(@"C:\Program Files\Punishing Gray Raven\2.2.2.0\Assets\KRApp.conf")), 99);
        //var str = Encoding.UTF8.GetString(result2);
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
        await Instance.Service.GetRequiredService<IAppContext<App>>().LauncherAsync(this);
        Instance.Service.GetService<IScreenCaptureService>().Register();
    }
}
