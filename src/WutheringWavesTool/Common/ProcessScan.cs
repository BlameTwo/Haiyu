namespace Haiyu.Common;

/// <summary>
/// 进程监控与扫描
/// </summary>
public  class ProcessScan
{
    public ProcessScan(string gameName,uint pid)
    {
        GameName = gameName;
        Pid = pid;
    }

    /// <summary>
    /// 游戏文件名称
    /// </summary>
    public string GameName { get; }

    /// <summary>
    /// 启动Pid   
    /// </summary>
    public uint Pid { get; }

    public void Scan()
    {

    }
}
