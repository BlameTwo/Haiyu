using Waves.Api.Models.Enums;

namespace Waves.Api.Models.Messanger;

public class GeeSuccessMessanger
{
    public GeeSuccessMessanger(string result,GeetType type)
    {
        Result = result;
        Type = type;
    }

    public string Result { get; }
    public GeetType Type { get; }
}
