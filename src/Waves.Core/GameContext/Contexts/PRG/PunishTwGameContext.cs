using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts.PRG;

public class PunishTwGameContext : GameContextBase
{

    internal PunishTwGameContext(GameAPIConfig config)
        : base(config, nameof(PunishTwGameContext)) { }


    public override Type ContextType => typeof(PunishTwGameContext);
    public override GameType GameType => GameType.Punish;
}
