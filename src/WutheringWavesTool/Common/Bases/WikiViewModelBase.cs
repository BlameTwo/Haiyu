using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiyu.Common.Bases;

public partial class WikiViewModelBase:ViewModelBase
{
    public WikiViewModelBase()
    {
        GameWikiClient = Instance.GetService<IGameWikiClient>();
        this.TipShow = Instance.GetService<ITipShow>();
    }

    public IGameWikiClient GameWikiClient { get; }
    public ITipShow TipShow { get; }
}
