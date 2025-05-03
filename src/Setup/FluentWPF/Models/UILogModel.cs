namespace FluentWPF.Models;

public class UILogModel
{
    public UILogModel(LogType type,string Message)
    {
        Type = type;
        this.Message = Message;
    }

    public UILogModel(Exception exception)
    {
        this.Type = LogType.ERR;
        this.Message = exception.Message;
    }

    public LogType Type { get; }
    public string Message { get; }
}



