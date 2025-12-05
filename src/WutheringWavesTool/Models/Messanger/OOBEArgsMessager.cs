namespace Haiyu.Models.Messanger;

public class OOBEArgsMessager
{
    /// <summary>
    /// 是否回退
    /// </summary>
    public bool IsBack { get; set; }

    /// <summary>
    /// 是否继续
    /// </summary>
    public bool IsNext { get; set; }


    /// <summary>
    /// 上一页
    /// </summary>
    public string NextPage { get; set; }

    /// <summary>
    /// 下一页
    /// </summary>
    public string ForwardPage { get; set; }
}