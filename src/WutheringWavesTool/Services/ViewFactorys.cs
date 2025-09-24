﻿namespace Haiyu.Services;

public class ViewFactorys : IViewFactorys
{
    public ViewFactorys(IAppContext<App> appContext)
    {
        AppContext = appContext;
    }

    public IAppContext<App> AppContext { get; }

    public GetGeetWindow CreateGeetWindow(GeetType type)
    {
        var windw = new GetGeetWindow(WindowNative.GetWindowHandle(AppContext.App.MainWindow),type);
        windw.Manager.MaxHeight = 510;
        windw.Manager.MaxWidth = 700;
        return windw;
    }

    public WindowModelBase ShowSignWindow(GameRoilDataItem role) =>
        this.ShowWindowBase<GamerSignPage>(role);

    public WindowModelBase ShowWindowBase<T>(object data)
        where T : UIElement, IWindowPage
    {
        var win = new WindowModelBase(WindowNative.GetWindowHandle(AppContext.App.MainWindow));
        var page = Instance.Service!.GetRequiredService<T>();
        if(data != null)
            page.SetData(data);
        page.SetWindow(win);
        win.Content = page;
        if (win.Content is FrameworkElement fs)
        {
            fs.RequestedTheme = ElementTheme.Dark;
        }
        return win;
    }

    public WindowModelBase ShowAdminDevice()
    {
        var win = new WindowModelBase(WindowNative.GetWindowHandle(AppContext.App.MainWindow));
        var page = Instance.Service!.GetRequiredService<DeviceInfoPage>();
        page.SetWindow(win);
        win.Content = page;
        if (win.Content is FrameworkElement fs)
        {
            fs.RequestedTheme = ElementTheme.Dark;
        }
        win.Manager.MaxHeight = 530;
        win.Manager.MaxWidth = 750;
        return win;
    }

    public WindowModelBase ShowRolesDataWindow(ShowRoleData detily)
    {
        var window = this.ShowWindowBase<GamerRoilsDetilyPage>(detily);
        window.Manager.MaxHeight = 530;
        window.Manager.MaxWidth = 750;
        return window;
    }


    public WindowModelBase ShowPlayerRecordWindow()
    {
        var window = this.ShowWindowBase<PlayerRecordPage>(null);
        window.Manager.MaxHeight = 700;
        window.Manager.MinHeight = 700;
        window.Manager.MaxWidth = 500;
        window.Manager.MinWidth = 500;
        return window;
    }

    public bool ShowToolWindow()
    {
        try
        {
            ToolWindow window = new ToolWindow();
            var content = Instance.GetService<ToolPage>();
            window.content.Content = content;
            window.Activate();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Window ShowAnalysisRecord()
    {

    }

    public TransparentWindow CreateTransperentWindow()
    {
        return new TransparentWindow();
    }

    public WindowModelBase ShowColorGame()
        =>ShowWindowBase<ColorFullGame>(null);
}
