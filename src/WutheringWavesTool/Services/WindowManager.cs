using Haiyu.Common.Contracts;
using Haiyu.Common.WindowContext;
using Haiyu.Models.Options;

namespace Haiyu.Services;

public sealed class WindowManager : IWindowManager
{
    public WindowManager(AppSettings appSettings)
    {
        AppSettings = appSettings;
    }

    public readonly Dictionary<string, WindowContext> _windowContext = new();

    public ShellWindowContext Shell
    {
        get
        {
            var context = _windowContext.GetValueOrDefault("Shell");
            if (context is ShellWindowContext shellC && shellC.Key == IWindowManager.ShellKey)
            {
                return shellC;
            }
            throw new ArgumentException("Shell window context not found.");
        }
    }

    public AppSettings AppSettings { get; }

    public async Task CreateShellWindowAsync()
    {
        WindowEx winEx = new WindowEx();

        winEx.Title = "Haiyu";
        winEx.AppWindow.SetIcon(AppDomain.CurrentDomain.BaseDirectory + "Assets/appLogo.ico");
        NativeWindowHelper.ForceDisableMaximize(winEx, targetDipWidth: 1150, targetDipHeight: 650);
        winEx.SystemBackdrop = new MicaBackdrop();
        (winEx.AppWindow.Presenter as OverlappedPresenter)!.SetBorderAndTitleBar(true, false);
        var shell = new ShellWindowContext(
            Instance.Host.Services.CreateAsyncScope(),
            IWindowManager.ShellKey
        );
        shell.SetWindow(winEx);
        shell.GetWindow().AppWindow.Closing += AppWindow_Closing;
        this._windowContext.Add(shell.Key, shell);
        #region Config
        var mainSizeConfig = await this.AppSettings.GetMainWindowSettingsAsync();
        if (mainSizeConfig == null)
        {
            mainSizeConfig = MainWindowSetting.Default;
        }
        #endregion
        #region Page
        if (await AppSettings.GetAutoOOBEAsync() == true)
        {
            var page = Instance.Host.Services.GetRequiredService<OOBEPage>();
            page.titlebar.Window = this.Shell.GetWindow();
            this.Shell.GetWindow().Content = page;
            this.Shell.GetWindow().ApplyWindowsOption(WindowsOption.OOBEWindowOption);
        }
        else
        {
            var page = Instance.Host.Services!.GetRequiredService<ShellPage>();
            page.titlebar.Window = this.Shell.GetWindow();
            this.Shell.GetWindow().Content = page;
            var defaultOption = WindowsOption.DefaultWindowsOption;
            var widthRate =
                double.IsFinite(mainSizeConfig.WidthRate) && mainSizeConfig.WidthRate > 0
                    ? mainSizeConfig.WidthRate
                    : MainWindowSetting.Default.WidthRate;
            var heightRate =
                double.IsFinite(mainSizeConfig.HeightRate) && mainSizeConfig.HeightRate > 0
                    ? mainSizeConfig.HeightRate
                    : MainWindowSetting.Default.HeightRate;

            this.Shell.GetWindow()
                .ApplyWindowsOption(
                    defaultOption with
                    {
                        Width = defaultOption.Width * widthRate,
                        Height = defaultOption.Height * heightRate,
                        IsResizable = mainSizeConfig.IsResize,
                    }
                );
        }
        #endregion
    }

    private void AppWindow_Closing(AppWindow sender, AppWindowClosingEventArgs args)
    {
        args.Cancel = true;
    }

    public void CreateWindow<T>(WindowManagerOption managerOption)
        where T : UIElement
    {
        ArgumentNullException.ThrowIfNull(managerOption);
        ArgumentException.ThrowIfNullOrWhiteSpace(managerOption.Key);

        if (_windowContext.TryGetValue(managerOption.Key, out var existingContext))
        {
            existingContext.Show();
            existingContext.GetWindow().Activate();
            return;
        }

        var scope = Instance.Host.Services.CreateAsyncScope();
        WindowContext? context = null;
        WindowEx? window = null;
        WindowSession? session = null;

        try
        {
            window = new WindowEx()
            {
                SystemBackdrop = new MicaBackdrop(),
            };
            window.ApplyWindowsOption(managerOption.WindowConfig);
            context = new WindowContext(scope, managerOption.Key)
            {
                Option = managerOption,
            };
           
            context.SetWindow(window);

            if (!_windowContext.TryAdd(context.Key, context))
            {
                throw new InvalidOperationException($"窗口 Key 已存在：{context.Key}");
            }

            session = scope.ServiceProvider.GetRequiredService<WindowSession>();

            session.Attach(window,context);

            var page = scope.ServiceProvider.GetRequiredService<T>();
            window.Content = page;

            window.Closed += Window_Closed;
            window.Activate();

            void Window_Closed(object sender, WindowEventArgs args)
            {
                window.Closed -= Window_Closed;
                session.Detach();

                if (_windowContext.Remove(context.Key, out var removedContext))
                {
                    removedContext.Dispose();
                }
            }
        }
        catch
        {
            if (context is not null)
            {
                _windowContext.Remove(context.Key);
            }

            session?.Detach();
            window?.Close();
            scope.Dispose();
            throw;
        }
    }

    public void CreateWindowBase<T>(WindowManagerOption managerOption, nint ownerId)
        where T : UIElement
    {
        ArgumentNullException.ThrowIfNull(managerOption);
        ArgumentException.ThrowIfNullOrWhiteSpace(managerOption.Key);

        if (_windowContext.TryGetValue(managerOption.Key, out var existingContext))
        {
            existingContext.Show();
            existingContext.GetWindow().Activate();
            return;
        }

        var scope = Instance.Host.Services.CreateAsyncScope();
        WindowModelContext? context = null;
        WindowModelBase? window = null;
        WindowSession? session = null;

        try
        {
            window = new WindowModelBase(ownerId, managerOption.WindowConfig)
            {
                SystemBackdrop = new MicaBackdrop(),
            };

            context = new WindowModelContext(scope, managerOption.Key)
            {
                OwnerId = ownerId,
                Option = managerOption,
            };
            context.SetWindow(window);

            if (!_windowContext.TryAdd(context.Key, context))
            {
                throw new InvalidOperationException($"窗口 Key 已存在：{context.Key}");
            }

            session = scope.ServiceProvider.GetRequiredService<WindowSession>();
            session.Attach(window,context);

            var page = scope.ServiceProvider.GetRequiredService<T>();
            window.Content = page;

            window.Closed += Window_Closed;
            window.AppWindow.Show();

            void Window_Closed(object sender, WindowEventArgs args)
            {
                window.Closed -= Window_Closed;
                session.Detach();

                if (_windowContext.Remove(context.Key, out var removedContext))
                {
                    removedContext.Dispose();
                }
            }
        }
        catch
        {
            if (context is not null)
            {
                _windowContext.Remove(context.Key);
            }

            session?.Detach();
            window?.Close();
            scope.Dispose();
            throw;
        }
    }

    public Task<IEnumerable<WindowContext>> GetWindowContextsAsync() =>
        Task.FromResult(_windowContext.Values.AsEnumerable());

    public bool IsWindowShow(string key)
    {
        var context = _windowContext.GetValueOrDefault(key);
        if (context is null)
            return false;
        var window = context.GetWindow();
        if (window is null)
            return false;
        return window.AppWindow.IsVisible;
    }

    public WindowContext? GetWindowContext(string key)
    {
        return _windowContext.GetValueOrDefault(key);
    }
}
