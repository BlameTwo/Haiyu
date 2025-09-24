namespace Haiyu.Models.Messanger;

public  class CloudLoginMessager
{
    public CloudLoginMessager(bool refresh,string userName)
    {
        Refresh = refresh;
        UserName = userName;
    }

    public bool Refresh { get; }
    public string UserName { get; }
}
