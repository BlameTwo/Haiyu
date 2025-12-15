using Haiyu.Plugin.Models;
using System;
using System.Threading.Tasks;

namespace Haiyu.Plugin.Contracts;

public interface ITool
{
    /// <summary>
    /// 工具名称
    /// </summary>
    public string ToolName { get; }

    /// <summary>
    /// 开始执行
    /// </summary>
    /// <param name="progress"></param>
    /// <returns></returns>
    public Task InvokeAsync(IProgress<ToolOutputArgs> progress);

    /// <summary>
    /// 工具警告提示信息
    /// </summary>
    public string ToolWaringString { get; }

}
