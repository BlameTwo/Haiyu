namespace Haiyu.Common;

public interface IAdaptive<Forward, Back>
{
    public Forward GetForward(Back value);

    public Back? GetBack(Forward forward);
}
