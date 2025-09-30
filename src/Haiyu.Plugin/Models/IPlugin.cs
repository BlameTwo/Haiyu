using System.Collections.Generic;
using System.Threading.Tasks;

namespace Haiyu.Plugin.Models;

public interface IPlugin
{
    public string Name { get;  }

    public Task LauncheAsync();

    public Dictionary<string,object> Paramters { get; set; }
}
