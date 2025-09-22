using Waves.Core.Models;

namespace Waves.Core.GameContext.Contexts;

public class GlobalPRGGameContext:GameContextBase
{
    public GlobalPRGGameContext(GameAPIConfig config)
    : base(config, nameof(GlobalPRGGameContext)) { }

    public override Type ContextType => typeof(GlobalPRGGameContext);
}
