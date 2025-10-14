using Waves.Api.Models.Enums;
using Waves.Api.Models.GameWikiiClient;

namespace Waves.Core.Contracts;

/// <summary>
/// 游戏wiki
/// </summary>
public interface IGameWikiClient
{
    Task<List<SideEventData?>> GetEventDataAsync(WikiType type, CancellationToken token = default);
}
