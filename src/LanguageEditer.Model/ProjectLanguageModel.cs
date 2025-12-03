using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace LanguageEditer.Model;

public  class ProjectLanguageModel
{
    /// <summary>
    /// 语言代码
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set;  }

    /// <summary>
    /// 语言名称（开发者自定义）
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set;  }

    [JsonPropertyName("resources")]
    public ObservableCollection<LanguageItem> Resources { get; set;  }
}
