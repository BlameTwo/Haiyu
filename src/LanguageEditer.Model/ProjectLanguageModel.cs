using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace LanguageEditer.Model;

public partial class ProjectLanguageModel:ObservableObject
{
    [JsonPropertyName("keys")]
    [ObservableProperty]
    public partial string Keys { get; set; }

    [JsonPropertyName("zh-Hans")]
    [ObservableProperty]
    public partial string ZH_Hans { get; set; }

    [JsonPropertyName("zh-Hant")]
    [ObservableProperty]
    public partial string ZH_Hant { get; set; }

    [JsonPropertyName("en-us")]
    [ObservableProperty]
    public partial string EN_Us { get; set;  }

    [JsonPropertyName("ja-Jp")]
    [ObservableProperty]
    public partial string Ja_Jp { get; set;  }
}

[JsonSerializable(typeof(ProjectLanguageModel))]
[JsonSerializable(typeof(List<ProjectLanguageModel>))]
[JsonSerializable(typeof(ObservableCollection<ProjectLanguageModel>))]
[JsonSerializable(typeof(List<LanguageItem>))]
[JsonSerializable(typeof(LanguageItem))]
public partial class ProjectLanguageModelContext:JsonSerializerContext
{

}