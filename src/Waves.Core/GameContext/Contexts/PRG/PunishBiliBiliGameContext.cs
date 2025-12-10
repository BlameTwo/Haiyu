
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public class PunishBiliBiliGameContext: KuroGameContextBase
{
    public PunishBiliBiliGameContext(GameAPIConfig config)
    : base(config, nameof(PunishBiliBiliGameContext)) { }

    public override Type ContextType => typeof(PunishBiliBiliGameContext);
    public override GameType GameType => GameType.Punish;
}
