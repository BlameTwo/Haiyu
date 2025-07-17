using Waves.Core.Models;

namespace Waves.Core.GameContext.Contexts;

/// <summary>
/// 战双国服
/// </summary>
public class MainPGRGameContext : GameContextBase
{
    public MainPGRGameContext(GameAPIConfig config)
        : base(config, nameof(MainPGRGameContext)) { }

    public override Type ContextType => typeof(MainPGRGameContext);
}
