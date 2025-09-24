namespace Haiyu.Common.Adaptives;

internal class GameFpsSettingAdaptive : IAdaptive<bool, string>
{
    public static GameFpsSettingAdaptive Default => new GameFpsSettingAdaptive();

    public string GetBack(bool forward)
    {
        return forward ? "True" : "False";
    }

    public bool GetForward(string value)
    {
        return value == "True" ? true : false;
    }
}
