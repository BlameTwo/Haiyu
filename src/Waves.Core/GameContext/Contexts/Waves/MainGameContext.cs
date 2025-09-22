using Waves.Core.Models;

namespace Waves.Core.GameContext.Contexts;

public class MainGameContext : GameContextBase
{
    internal MainGameContext(GameAPIConfig config)
        : base(config, nameof(MainGameContext)) { }

    public override Type ContextType => typeof(MainGameContext);
}
