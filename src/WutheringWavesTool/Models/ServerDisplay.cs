namespace Haiyu.Models;

public class ServerDisplay
{
    public string Display { get; set; }

    public string Tag { get; set; }

    public string Key { get; set; }

    /// <summary>
    /// 是否显示公告面板
    /// </summary>
    public bool ShowCard { get; set;  }

    public static ObservableCollection<ServerDisplay> GetWavesGames =>
        [
            new ServerDisplay()
            {
                Display = "官服",
                Key = $"{nameof(WavestMainGameContext)}",
                Tag = "Main",
                ShowCard= true
            },new ServerDisplay()
            {
                Display = "Bilibili",
                Key = $"{nameof(WavesBiliBiliGameContext)}",
                Tag = "Main",
                ShowCard= false
            },new ServerDisplay()
            {
                Display = "国际服",
                Key = $"{nameof(WavesGlobalGameContext)}",
                Tag = "Main",
                ShowCard= true
            },
        ];
}
