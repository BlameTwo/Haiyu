namespace WutheringWavesTool.Services.Contracts;

public interface IAppContext<T>
    where T : ClientApplication
{
    public T App { get; }

    public Controls.TitleBar MainTitle { get; }
    public IWallpaperService WallpaperService { get; }
    public Task LauncherAsync(T app);

    public Task TryInvokeAsync(Func<Task> action);

    public SolidColorBrush StressColor { get; }
    public Color StressShadowColor { get; }
    public SolidColorBrush StessForground { get; }
    public void TryInvoke(Action action);

    public ElementTheme CurrentElement { get; set; }

    void SetTitleControl(Controls.TitleBar titleBar);

    public void SetElementTheme(ElementTheme theme);
}
