using System.Text.Json.Serialization;

namespace LanguageEditer.Model;

public  class LanguageItem
{
    
    [JsonPropertyName("key")]
    public string Key { get; set;  }

    [JsonPropertyName("value")]
    public string Value { get; set;  }
}


