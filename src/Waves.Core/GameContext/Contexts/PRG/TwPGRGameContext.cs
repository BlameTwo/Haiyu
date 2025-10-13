using Waves.Core.Models;

namespace Waves.Core.GameContext.Contexts.PRG;

public class TwPGRGameContext : GameContextBase
{

    internal TwPGRGameContext(GameAPIConfig config)
        : base(config, nameof(TwPGRGameContext)) { }


    public override Type ContextType => typeof(TwPGRGameContext);
}
