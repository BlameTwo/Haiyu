using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

/// <summary>
/// 战双国服
/// </summary>
public class PunishMainGameContext : GameContextBase
{
    public PunishMainGameContext(GameAPIConfig config)
        : base(config, nameof(PunishMainGameContext)) { }

    public override Type ContextType => typeof(PunishMainGameContext);
    public override GameType GameType => GameType.Punish;
}
