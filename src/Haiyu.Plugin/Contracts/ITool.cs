using Haiyu.Plugin.Models;
using Haiyu.Plugin.Models.Enums;
using System;
using System.Threading.Tasks;

namespace Haiyu.Plugin.Contracts;

public interface ITool
{
    /// <summary>
    /// 工具名称
    /// </summary>
    public string ToolName { get; }

    public ToolType Status { get; }

    /// <summary>
    /// 工具警告提示信息
    /// </summary>
    public string ToolWaringString { get; }

}
