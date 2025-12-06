using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public class WavesMainGameContext : GameContextBase
{
    internal WavesMainGameContext(GameAPIConfig config)
        : base(config, nameof(WavesMainGameContext)) { }

    public override Type ContextType => typeof(WavesMainGameContext);
    public override GameType GameType => GameType.Waves;
}
