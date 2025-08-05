namespace WutheringWavesTool.Common;

public interface IResultDialog<T> : IDialog
{
    public T? GetResult();
}
