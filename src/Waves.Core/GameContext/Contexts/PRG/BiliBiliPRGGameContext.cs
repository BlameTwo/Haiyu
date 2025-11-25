
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public class BiliBiliPRGGameContext: GameContextBase
{
    public BiliBiliPRGGameContext(GameAPIConfig config)
    : base(config, nameof(BiliBiliPRGGameContext)) { }

    public override Type ContextType => typeof(GlobalPRGGameContext);
    public override GameType GameType => GameType.Punish;
}
