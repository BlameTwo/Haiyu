namespace Haiyu.Models.Args;

/// <summary>
/// 创建抽卡记录参数
/// </summary>
public class CreateRecordArgs
{
    public CreateRecordType Type { get; }

    public string? Link { get; set; }

    public RecordCacheDetily? Cache { get; set; }

    public CreateRecordArgs(CreateRecordType type)
    {
        Type = type;
    }
}

public enum CreateRecordType : uint
{
    None = 0,
    Create = 1,
    SelectItemOpen = 2,
    Update = 3,
}
