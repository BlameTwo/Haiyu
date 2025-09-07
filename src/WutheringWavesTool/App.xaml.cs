

using WutheringWavesTool.Helpers;

namespace WutheringWavesTool;

public partial class App : ClientApplication
{
    public static string BassFolder =>
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Waves";

    public static string RecordFolder => BassFolder + "\\RecordCache";

    public static string WrallpaperFolder => BassFolder + "\\WallpaperImages";

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
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        this.UnhandledException += App_UnhandledException;
        Directory.CreateDirectory(BassFolder);
        Directory.CreateDirectory(RecordFolder);
        Directory.CreateDirectory(WrallpaperFolder);
        GameContextFactory.GameBassPath = BassFolder;
        Instance.InitService();
        this.InitializeComponent();
        var id = DeviceNumGenerator.GenerateId();
        Directory.CreateDirectory(AppSettings.CloudFolderPath);
    }

    private void CurrentDomain_UnhandledException(
        object sender,
        System.UnhandledExceptionEventArgs e
    ) { }

    private void App_UnhandledException(
        object sender,
        Microsoft.UI.Xaml.UnhandledExceptionEventArgs e
    )
    {
        e.Handled = true;
        File.WriteAllText("D:\\output.log", e.Message);
    }

    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        await Instance.Service.GetRequiredService<IAppContext<App>>().LauncherAsync(this);
    }
}
