namespace Haiyu.Services.Contracts;

public interface ITipShow
{
    /// <summary>
    /// 弹出消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="icon"></param>
    void ShowMessage(string message, Symbol icon);

    /// <summary>
    /// 父对象
    /// </summary>
    Panel Owner { get; set; }
}
