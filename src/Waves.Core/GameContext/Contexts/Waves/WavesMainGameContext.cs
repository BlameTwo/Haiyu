using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext.Contexts;

public sealed class WavesMainGameContext : KuroGameContextBase
{
    public override string GameContextNameKey => nameof(WavesMainGameContext);
    internal WavesMainGameContext(GameAPIConfig config)
        : base(config, nameof(WavesMainGameContext)) { }

    public override Type ContextType => typeof(WavesMainGameContext);
    public override GameType GameType => GameType.Waves;
}
