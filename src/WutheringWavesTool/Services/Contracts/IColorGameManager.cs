using Haiyu.Models.ColorGames;

namespace Haiyu.Services.Contracts;

public interface IColorGameManager
{
    public Task<(bool, string)> SaveGameAsync(ColorInfo info, string currentFile, CancellationToken token = default);

    public Task<List<ColorInfo>> GetGamesAsync(CancellationToken token = default);
}
