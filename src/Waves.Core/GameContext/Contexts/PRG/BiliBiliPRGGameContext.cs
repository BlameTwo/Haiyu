
using Waves.Core.Models;

namespace Waves.Core.GameContext.Contexts;

public class BiliBiliPRGGameContext: GameContextBase
{
    public BiliBiliPRGGameContext(GameAPIConfig config)
    : base(config, nameof(BiliBiliPRGGameContext)) { }

    public override Type ContextType => typeof(GlobalPRGGameContext);
}
