namespace Haiyu.Models;

public class MainNavigationArgs
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 导航Key
    /// </summary>
    public string NavigationKey { get; set; }

    /// <summary>
    /// 导航Value
    /// </summary>
    public string NavigationValue { get; set; }

    public ObservableCollection<MainNavigationArgs> GetDefault()
    {
        ObservableCollection<MainNavigationArgs> args = new();
        if (AppSettings.ShowWavesMainGame == true)
        {
            args.Add(
                new()
                {
                    Icon = "ms-appx:///Assets/GameIcons/global.ico",
                    Title = "鸣潮（官方）",
                    NavigationKey = "Haiyu.ViewModel.GameViewModels.MainGameViewModel",
                    NavigationValue = null,
                }
            );
        }
        return args;
    }
}
