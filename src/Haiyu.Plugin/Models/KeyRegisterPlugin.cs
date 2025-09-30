using System.Collections.Generic;
using System.Threading.Tasks;

namespace Haiyu.Plugin.Models;

public class KeyRegisterPlugin : IPlugin
{
    public string Name => "键位注册";

    public Dictionary<string, object> Paramters { get; set; }

    public virtual Task LauncheAsync()
    {
        return Task.CompletedTask;
    }
}
