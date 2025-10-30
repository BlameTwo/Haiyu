using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Waves.Core.Services;
using Haiyu.Services.DialogServices;
using TitleBar = Haiyu.Controls.TitleBar;
using Waves.Core.GameContext.Contexts.PRG;

namespace Haiyu.Services;

public class AppContext<T> : IAppContext<T>
    where T : ClientApplication
{
    public AppContext(
        IWavesClient wavesClient,
        IWallpaperService wallpaperService,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        [FromKeyedServices("AppLog")] LoggerService loggerService
    )
    {
        WavesClient = wavesClient;
        WallpaperService = wallpaperService;
        DialogManager = dialogManager;
        LoggerService = loggerService;
        WallpaperService.WallpaperPletteChanged += WallpaperService_WallpaperPletteChanged;
    }

    private ContentDialog _dialog;

    public T App { get; private set; }

    public IWavesClient WavesClient { get; }
    public IWallpaperService WallpaperService { get; }
    public IDialogManager DialogManager { get; }
    public LoggerService LoggerService { get; }

    public async Task LauncherAsync(T app)
    {
        try
        {
            await Instance.GetService<IWavesClient>().InitAsync();
            await Instance
                .Service!.GetRequiredKeyedService<IGameContext>(nameof(MainGameContext))
                .InitAsync();
            await Instance
                .Service!.GetRequiredKeyedService<IGameContext>(nameof(BiliBiliGameContext))
                .InitAsync();
            await Instance
                .Service!.GetRequiredKeyedService<IGameContext>(nameof(GlobalGameContext))
                .InitAsync();
            await Instance
                .Service!.GetRequiredKeyedService<IGameContext>(nameof(MainPGRGameContext))
                .InitAsync();
            await Instance
                .Service!.GetRequiredKeyedService<IGameContext>(nameof(BiliBiliPRGGameContext))
                .InitAsync();
            await Instance
                .Service!.GetRequiredKeyedService<IGameContext>(nameof(GlobalPRGGameContext))
                .InitAsync();
            await Instance
                .Service!.GetRequiredKeyedService<IGameContext>(nameof(TwPGRGameContext))
                .InitAsync();
            this.App = app;
            var win = new MainWindow();
            var page = Instance.Service!.GetRequiredService<ShellPage>();
            page.titlebar.Window = win;
            win.Content = page;
            win.Activate();
            try
            {
                var scale = TitleBar.GetScaleAdjustment(win);
                int targetDipWidth = 1150;
                int targetDipHeight = 650;
                var pixelWidth = (int)Math.Round(targetDipWidth * scale);
                var pixelHeight = (int)Math.Round(targetDipHeight * scale);
                win.AppWindow.Resize(
                    new Windows.Graphics.SizeInt32 { Width = pixelWidth, Height = pixelHeight }
                );
            }
            catch
            {
                // Fallback to logical size if DPI detection fails
                win.MaxWidth = 1100;
                win.MaxHeight = 700;
            }

            win.IsResizable = false;
            win.IsMaximizable = false;
            this.App.MainWindow = win;
            (win.AppWindow.Presenter as OverlappedPresenter)!.SetBorderAndTitleBar(true, false);
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
        catch (Exception ex)
        {
            LoggerService.WriteError(ex.Message);
            Process.GetCurrentProcess().Kill();
        }
       
    }

    private void AppWindow_Closing(
        Microsoft.UI.Windowing.AppWindow sender,
        Microsoft.UI.Windowing.AppWindowClosingEventArgs args
    )
    {
        args.Cancel = true;
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

    public Controls.TitleBar MainTitle { get; private set; }
    public SolidColorBrush StressColor { get; private set; } = new(Colors.DodgerBlue);
    public Color StressShadowColor { get; private set; } = Colors.AliceBlue;
    public SolidColorBrush StessForground { get; private set; } = new(Colors.Black);

    public void SetTitleControl(Controls.TitleBar titleBar)
    {
        this.MainTitle = titleBar;
    }

    public void TryInvoke(Action action)
    {
        this.App.MainWindow.DispatcherQueue.TryEnqueue(() => action.Invoke());
    }

    public void Minimise()
    {
        this.App.MainWindow.Minimize();
    }

    public async Task CloseAsync()
    {
        var close = AppSettings.CloseWindow;
        if (close == "True")
        {
            Environment.Exit(0);
        }
        else if (close == "False")
        {
            this.App.MainWindow.Hide();
        }
        else
        {
            var result = await DialogManager.ShowCloseWindowResult();
            if (result.IsExit)
            {
                Environment.Exit(0);
            }
            else
            {
                this.App.MainWindow.Hide();
            }
        }
    }
}
