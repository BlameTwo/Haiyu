namespace WutheringWavesTool.Services;

public class ViewFactorys : IViewFactorys
{
    public ViewFactorys(IAppContext<App> appContext)
    {
        AppContext = appContext;
    }

    public IAppContext<App> AppContext { get; }

    public GetGeetWindow CreateGeetWindow()
    {
        var windw = new GetGeetWindow();
        windw.MaxHeight = 510;
        windw.MaxWidth = 700;
        return windw;
    }

    public WindowModelBase ShowSignWindow(GameRoilDataItem role) =>
        this.ShowWindowBase<GamerSignPage>(role);

    public WindowModelBase ShowWindowBase<T>(object data)
        where T : UIElement, IWindowPage
    {
        var win = new WindowModelBase();
        var page = Instance.Service!.GetRequiredService<T>();
        page.SetData(data);
        page.SetWindow(win);
        win.IsTitleBarVisible = true;
        win.Content = page;
        if (win.Content is FrameworkElement fs)
        {
            fs.RequestedTheme =
                AppSettings.AppTheme == null ? ElementTheme.Default
                : AppSettings.AppTheme == "Dark" ? ElementTheme.Dark
                : AppSettings.AppTheme == "Light" ? ElementTheme.Light
                : ElementTheme.Default;
        }
        return win;
    }

    public WindowModelBase ShowRolesDataWindow(ShowRoleData detily)
    {
        var window = this.ShowWindowBase<GamerRoilsDetilyPage>(detily);
        window.MaxHeight = 530;
        window.MaxWidth = 750;
        return window;
    }

    public WindowModelBase ShowPlayerRecordWindow()
    {
        var window = this.ShowWindowBase<PlayerRecordPage>(null);
        window.MaxHeight = 700;
        window.MinHeight = 700;
        window.MaxWidth = 500;
        window.MinWidth = 500;
        return window;
    }
}
