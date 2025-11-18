namespace Haiyu.Models.Messanger
{
    public class SwitchRoleMessager
    {
        public GameRoilDataWrapper Data { get; set; }

        public SwitchRoleMessager(GameRoilDataWrapper data)
        {
            Data = data;
        }
    }
}
