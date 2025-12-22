using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts
{
    public sealed class WavesGlobalGameContext : KuroGameContextBase
    {
        public override string GameContextNameKey => nameof(WavesGlobalGameContext);
        internal WavesGlobalGameContext(GameAPIConfig config)
            : base(config, nameof(WavesGlobalGameContext)) { }

        public override Type ContextType => typeof(WavesGlobalGameContext);
        public override GameType GameType => GameType.Waves;
    }
}
