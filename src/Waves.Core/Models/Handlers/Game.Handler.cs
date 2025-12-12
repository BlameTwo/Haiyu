using Waves.Core.Models;

namespace Waves.Core.GameContext;

public delegate void GameVerifyDelegate();

public delegate void GameDownloadDelegate();

public delegate Task GameContextOutputDelegate(object sender, GameContextOutputArgs args);

public delegate Task GameContextProdOutputDelegate(object sender, GameContextOutputArgs args);

partial class KuroGameContextBase
{
    private GameContextOutputDelegate? gameContextOutputDelegate;

    public event GameContextOutputDelegate GameContextOutput
    {
        add => gameContextOutputDelegate += value;
        remove => gameContextOutputDelegate -= value;
    }

    private GameContextProdOutputDelegate? gameContextProdOutputDelegate;
    public event GameContextProdOutputDelegate GameContextProdOutput
    {
        add => gameContextProdOutputDelegate += value;
        remove => gameContextProdOutputDelegate -= value;
    }
}
