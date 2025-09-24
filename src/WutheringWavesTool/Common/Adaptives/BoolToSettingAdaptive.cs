namespace Haiyu.Common.Adaptives
{
    public class BoolToSettingAdaptive : IAdaptive<bool, string>
    {
        public static BoolToSettingAdaptive Instance { get; } = new();

        public string GetBack(bool forward)
        {
            return forward.ToString();
        }

        public bool GetForward(string value)
        {
            return Convert.ToBoolean(value);
        }
    }
}
