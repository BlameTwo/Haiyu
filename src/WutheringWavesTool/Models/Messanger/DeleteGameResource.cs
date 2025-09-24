namespace Haiyu.Models.Messanger;

public class DeleteGameResource
{
    public DeleteGameResource(bool isMainResource, string gameContextName)
    {
        IsMainResource = isMainResource;
        GameContextName = gameContextName;
    }

    public bool IsMainResource { get; }
    public string GameContextName { get; }
}
