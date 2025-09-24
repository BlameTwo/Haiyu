namespace Haiyu.Common;

public interface ICommunityViewModel : IDisposable
{
    public Task SetDataAsync(GameRoilDataItem item);
}
