namespace Haiyu.Common.Contracts;

/// <summary>
/// 表示窗口需要在 WindowSession 完成绑定后执行初始化。
/// </summary>
public interface IWindowInitializable
{
    void Initialize();

    void Dispose();
}
