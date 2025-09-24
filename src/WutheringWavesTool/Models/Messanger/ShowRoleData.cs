namespace Haiyu.Models.Messanger;

public class ShowRoleData
{
    public long Id { get; set; }
    public GameRoilDataItem GameRoilDataItem { get; }

    public ShowRoleData(long id, GameRoilDataItem gameRoilDataItem)
    {
        Id = id;
        GameRoilDataItem = gameRoilDataItem;
    }
}
