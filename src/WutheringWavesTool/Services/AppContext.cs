using CommunityToolkit.WinUI;
using H.NotifyIcon;
using Microsoft.UI.Dispatching;
using Waves.Core.Services;
using WavesLauncher.Core.Contracts;
using Windows.ApplicationModel.Contacts.DataProvider;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.Services;

public class AppContext<T> : IAppContext<T>
    where T : ClientApplication
{
    public AppContext(IWavesClient wavesClient, IWallpaperService wallpaperService, [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager)
    {
        WavesClient = wavesClient;
        WallpaperService = wallpaperService;
        DialogManager = dialogManager;
        WallpaperService.WallpaperPletteChanged += WallpaperService_WallpaperPletteChanged;
    }

    private ContentDialog _dialog;

    public T App { get; private set; }


    public IWavesClient WavesClient { get; }
    public IWallpaperService WallpaperService { get; }
    public IDialogManager DialogManager { get; }

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
        var page = Instance.Service!.GetRequiredService<ShellPage>();
        page.titlebar.Window = win;
        win.Content = page;
        win.MaxWidth = 1100;
        win.MaxHeight = 700;
        win.IsResizable = false;
        win.IsMaximizable = false;
        this.App.MainWindow = win;
        (win.AppWindow.Presenter as OverlappedPresenter)!.SetBorderAndTitleBar(true, false);
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

    public async Task Close()
    {
        var close = AppSettings.CloseWindow;
        if(close == "True")
        {
            Environment.Exit(0);
            
        }
        else if(close == "False")
        {
            this.App.MainWindow.Hide(true);
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
                this.App.MainWindow.Hide(true);
            }
        }
        
        
    }
}
