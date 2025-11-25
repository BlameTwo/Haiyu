using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts.PRG;

public class TwPGRGameContext : GameContextBase
{

    internal TwPGRGameContext(GameAPIConfig config)
        : base(config, nameof(TwPGRGameContext)) { }


    public override Type ContextType => typeof(TwPGRGameContext);
    public override GameType GameType => GameType.Punish;
}
