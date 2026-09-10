using Haiyu.Common.Contracts;
using Haiyu.Common.WindowContext;
using Haiyu.Models.Options;

namespace Haiyu.Services.Contracts;

public interface IWindowManager
{
    public static string ShellKey => "Shell";

    public Task<IEnumerable<WindowContext>> GetWindowContextsAsync();

    /// <summary>
    /// 主窗口
    /// </summary>
    public ShellWindowContext Shell { get; }

    public AppSettings AppSettings { get; }

    /// <summary>
    /// 创建主窗口Shell
    /// </summary>
    public Task CreateShellWindowAsync();

    public Task RemoveShellWindowAsync();

    public bool IsWindowShow(string key);

    /// <summary>
    /// 创建普通窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="managerOption"></param>
    public void CreateWindow<T>(WindowManagerOption managerOption)
        where T : UIElement;

    public Task<TResult?> CreateWindowAsync<T, TResult>(WindowManagerOption managerOption)
        where T : UIElement;

    /// <summary>
    /// 创建模态窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="managerOption"></param>
    /// <param name="ownerId"></param>
    public void CreateWindowBase<T>(WindowManagerOption managerOption, nint ownerId)
        where T : UIElement;

    public Task<TResult?> CreateWindowBaseAsync<T, TResult>(
        WindowManagerOption managerOption,
        nint ownerId)
        where T : UIElement;

    /// <summary>
    /// 原始窗口创建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="managerOption"></param>
    public void CreateOriginWindow<T>(WindowManagerOption managerOption)
        where T : Window;

    public Task<TResult?> CreateOriginWindowAsync<T, TResult>(WindowManagerOption managerOption)
        where T : Window;

    public WindowSession CreateTransparentWindow<T>(PostionTransparentWindowOption managerOption)
        where T : UIElement;

    public WindowContext? GetWindowContext(string key);
}
