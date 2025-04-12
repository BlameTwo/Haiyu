﻿using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Waves.Core.Services;
using WavesLauncher.Core.Contracts;

namespace WutheringWavesTool.Services;

public class AppContext<T> : IAppContext<T>
    where T : ClientApplication
{
    public AppContext(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
    }

    private ContentDialog _dialog;

    public T App { get; private set; }

    public IWavesClient WavesClient { get; }

    public async Task LauncherAsync(T app)
    {
        await Instance
            .Service!.GetRequiredKeyedService<IGameContext>(nameof(MainGameContext))
            .InitAsync();
        //await Instance
        //    .Service!.GetRequiredKeyedService<IGameContext>(nameof(BilibiliGameContext))
        //    .InitAsync();
        //await Instance
        //    .Service!.GetRequiredKeyedService<IGameContext>(nameof(GlobalGameContext))
        //    .InitAsync();
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

        win.Activate();

        if (win.Content is FrameworkElement fe)
        {
            fe.RequestedTheme =
                AppSettings.AppTheme == null ? ElementTheme.Default
                : AppSettings.AppTheme == "Dark" ? ElementTheme.Dark
                : AppSettings.AppTheme == "Light" ? ElementTheme.Light
                : ElementTheme.Default;
        }
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

    public void SetElementTheme(ElementTheme theme)
    {
        if (this.App.MainWindow.Content is FrameworkElement fe)
        {
            fe.RequestedTheme = theme;
            this.CurrentElement = theme;
        }
    }

    public void SetTitleControl(Controls.TitleBar titleBar)
    {
        this.MainTitle = titleBar;
    }

    public void TryInvoke(Action action)
    {
        this.App.MainWindow.DispatcherQueue.TryEnqueue(() => action.Invoke());
    }
}
