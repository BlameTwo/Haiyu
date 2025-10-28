using Waves.Api.Models.Launcher;

namespace Haiyu.Models.Dialogs;

public class GameLauncherCacheArgs
{
    public string GameContextName { get; set; }

    public List<KRSDKLauncherCache> Datas { get; set;  }
}
