using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public class PunishGlobalGameContext:KuroGameContextBase
{
    public PunishGlobalGameContext(GameAPIConfig config)
    : base(config, nameof(PunishGlobalGameContext)) { }

    public override Type ContextType => typeof(PunishGlobalGameContext);
    public override GameType GameType => GameType.Punish;
}
