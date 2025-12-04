namespace Haiyu.Services.Contracts;

public interface IThemeService
{
    public void SetTheme(ElementTheme? theme = null);

    public ElementTheme CurrentTheme { get; }
}
