namespace LanguageEditer.Model;

public class UILanguageModel
{
    /// <summary>
    /// 资源Key
    /// </summary>
    public string Keys { get; set;}

    /// <summary>
    /// 项目代码
    /// </summary>
    public string Code { get; set;}    
    /// <summary>
    /// 项目名称
    /// </summary>
    public string Name { get; set;}

    /// <summary>
    /// 语言列
    /// </summary>
    public List<string> Colums { get; set; }
}
