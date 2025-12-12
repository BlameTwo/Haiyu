using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts
{
    public class WavesGlobalGameContext : KuroGameContextBase
    {
        internal WavesGlobalGameContext(GameAPIConfig config)
            : base(config, nameof(WavesGlobalGameContext)) { }

        public override Type ContextType => typeof(WavesGlobalGameContext);
        public override GameType GameType => GameType.Waves;
    }
}
