using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public class MainGameContext : GameContextBase
{
    internal MainGameContext(GameAPIConfig config)
        : base(config, nameof(MainGameContext)) { }

    public override Type ContextType => typeof(MainGameContext);
    public override GameType GameType => GameType.Waves;
}
