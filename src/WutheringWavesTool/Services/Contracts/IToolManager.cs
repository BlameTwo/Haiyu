using Windows.Graphics;

namespace Haiyu.Services.Contracts;

public interface IToolManager
{
    public void SetPostionValue(string key, SizeInt32 size);

    public void RegisterTool(string key);

    public void ShowGamerTool();
}
