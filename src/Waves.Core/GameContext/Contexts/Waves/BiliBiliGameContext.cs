using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waves.Core.Models;

namespace Waves.Core.GameContext.Contexts
{
    public class BiliBiliGameContext : GameContextBase
    {
        internal BiliBiliGameContext(GameAPIConfig config)
            : base(config, nameof(BiliBiliGameContext)) { }

        public override Type ContextType => typeof(BiliBiliGameContext);
    }
}
