using Windows.Graphics;

namespace WutheringWavesTool.Services;

public sealed class ToolManager : IToolManager
{
    public static readonly HashSet<string> ToolNames = new HashSet<string>();

    public void RegisterTool(string key)
    {
        if (ToolNames.Contains(key))
            return;
        ToolNames.Add(key);
    }

    public void SetPostionValue(string key, SizeInt32 size)
    {
        throw new NotImplementedException();
    }

    public void ShowGamerTool()
    {
        throw new NotImplementedException();
    }
}
