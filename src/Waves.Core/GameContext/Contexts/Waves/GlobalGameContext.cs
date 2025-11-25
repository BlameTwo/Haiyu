using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts
{
    public class GlobalGameContext : GameContextBase
    {
        internal GlobalGameContext(GameAPIConfig config)
            : base(config, nameof(GlobalGameContext)) { }

        public override Type ContextType => typeof(GlobalGameContext);
        public override GameType GameType => GameType.Waves;
    }
}
