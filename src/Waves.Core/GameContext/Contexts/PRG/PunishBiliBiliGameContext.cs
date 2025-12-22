
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public sealed class PunishBiliBiliGameContext: KuroGameContextBase
{
    public override string GameContextNameKey => nameof(PunishBiliBiliGameContext);
    public PunishBiliBiliGameContext(GameAPIConfig config)
    : base(config, nameof(PunishBiliBiliGameContext)) { }

    public override Type ContextType => typeof(PunishBiliBiliGameContext);
    public override GameType GameType => GameType.Punish;
}
