using Waves.Api.Models.Enums;
using Waves.Api.Models.GameWikiiClient;

namespace Waves.Core.Contracts;

/// <summary>
/// 游戏wiki
/// </summary>
public interface IGameWikiClient
{
    public Task<WikiHomeModel?> GetHomePageAsync(
        WikiType type,
        CancellationToken token = default
    );

    Task<List<HotContentSide?>> GetEventDataAsync(WikiType type, CancellationToken token = default);

    public Task<EventContentSide?> GetEventTabDataAsync(
        WikiType type,
        CancellationToken token = default
    );
}
