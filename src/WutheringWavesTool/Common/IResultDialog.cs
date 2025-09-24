namespace Haiyu.Common;

public interface IResultDialog<T> : IDialog
{
    public T? GetResult();
}
