using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public class WavestMainGameContext : GameContextBase
{
    internal WavestMainGameContext(GameAPIConfig config)
        : base(config, nameof(WavestMainGameContext)) { }

    public override Type ContextType => typeof(WavestMainGameContext);
    public override GameType GameType => GameType.Waves;
}
