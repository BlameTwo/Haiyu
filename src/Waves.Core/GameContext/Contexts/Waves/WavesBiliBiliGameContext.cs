using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts
{
    public class WavesBiliBiliGameContext : GameContextBase
    {
        internal WavesBiliBiliGameContext(GameAPIConfig config)
            : base(config, nameof(WavesBiliBiliGameContext)) { }

        public override Type ContextType => typeof(WavesBiliBiliGameContext);

        public override GameType GameType =>  GameType.Waves;
    }
}
