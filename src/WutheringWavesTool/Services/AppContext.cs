using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Waves.Core.Services;
using WavesLauncher.Core.Contracts;
using Windows.ApplicationModel.Contacts.DataProvider;

namespace WutheringWavesTool.Services;

public class AppContext<T> : IAppContext<T>
    where T : ClientApplication
{
    public AppContext(IWavesClient wavesClient, IWallpaperService wallpaperService)
    {
        WavesClient = wavesClient;
        WallpaperService = wallpaperService;
        WallpaperService.WallpaperPletteChanged += WallpaperService_WallpaperPletteChanged;
        ThemeService = new ThemeService();
    }

    private ContentDialog _dialog;

    public T App { get; private set; }

    public IThemeService ThemeService { get; }

    public IWavesClient WavesClient { get; }
    public IWallpaperService WallpaperService { get; }

    public async Task LauncherAsync(T app)
    {
        await Instance
            .Service!.GetRequiredKeyedService<IGameContext>(nameof(MainGameContext))
            .InitAsync();
        await Instance
            .Service!.GetRequiredKeyedService<IGameContext>(nameof(BiliBiliGameContext))
            .InitAsync();
        await Instance
            .Service!.GetRequiredKeyedService<IGameContext>(nameof(GlobalGameContext))
            .InitAsync();
        this.App = app;
        var win = new MainWindow();
        ThemeService.Initialize(win);
        var page = Instance.Service!.GetRequiredService<ShellPage>();
        page.titlebar.Window = win;
        win.Content = page;
        win.MaxWidth = 1100;
        win.MaxHeight = 700;
        win.IsResizable = false;
        win.IsMaximizable = false;
        this.App.MainWindow = win;
        win.Activate();
        if (await WavesClient.IsLoginAsync())
        {
            var gamers = await WavesClient.GetWavesGamerAsync();
            if (gamers != null && gamers.Success)
            {
                foreach (var item in gamers.Data)
                {
                    var data = await WavesClient.RefreshGamerDataAsync(item);
                }
            }
        }
        this.App.MainWindow.AppWindow.Closing += AppWindow_Closing;
    }

    private void AppWindow_Closing(
        Microsoft.UI.Windowing.AppWindow sender,
        Microsoft.UI.Windowing.AppWindowClosingEventArgs args
    )
    {
        args.Cancel = true;
        var mainContext = Instance.Service!.GetRequiredKeyedService<IGameContext>(
            nameof(MainGameContext)
        );
        //var biliContext = Instance.Service!.GetRequiredKeyedService<IGameContext>(
        //    nameof(BilibiliGameContext)
        //);
        //var globalContext = Instance.Service!.GetRequiredKeyedService<IGameContext>(
        //    nameof(GlobalGameContext)
        //);
        Process.GetCurrentProcess().Kill();
    }

    public async Task TryInvokeAsync(Func<Task> action)
    {
        await SafeInvokeAsync(
                this.App.MainWindow.DispatcherQueue,
                action,
                priority: Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal
            )
            .ConfigureAwait(false);
    }

    private void WallpaperService_WallpaperPletteChanged(object sender, PletteArgs color)
    {
        if (color.Background == null || color.Forground == null || color.Shadow == null)
            return;
        this.StressColor = new SolidColorBrush(color.Background.Value);
        this.StressShadowColor = color.Shadow.Value;
        this.StessForground = new SolidColorBrush(color.Forground.Value);
    }

    async Task SafeInvokeAsync(
        DispatcherQueue dispatcher,
        Func<Task> action,
        DispatcherQueuePriority priority = DispatcherQueuePriority.Normal
    )
    {
        try
        {
            if (dispatcher.HasThreadAccess)
            {
                await action().ConfigureAwait(false);
            }
            else
            {
                await dispatcher.EnqueueAsync(action, priority).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UI操作失败: {ex.Message}");
        }
    }

    public ElementTheme CurrentElement { get; set; }
    public Controls.TitleBar MainTitle { get; private set; }
    public SolidColorBrush StressColor { get; private set; } = new(Colors.DodgerBlue);
    public Color StressShadowColor { get; private set; } = Colors.AliceBlue;
    public SolidColorBrush StessForground { get; private set; } = new(Colors.Black);

    public void SetElementTheme(ElementTheme theme)
    {
        ThemeService.SetElementTheme(theme);
    }

    public void SetTitleControl(Controls.TitleBar titleBar)
    {
        this.MainTitle = titleBar;
    }

    public void TryInvoke(Action action)
    {
        this.App.MainWindow.DispatcherQueue.TryEnqueue(() => action.Invoke());
    }

    public void InvokeMin()
    {
        if (this.App.MainWindow != null)
        {
            App.MainWindow.Minimize();
        }
    }
}
