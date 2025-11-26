using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Waves.Api.Models.GameWikiiClient;

public class WikiCatalogue
{
    [JsonPropertyName("children")]
    public List<WikiCatalogueChildren> Childrens { get; set; }
}


public class WikiCatalogueChildren
{
    [JsonPropertyName("name")]
    public string Name { get; set;  }

    [JsonPropertyName("key")]
    public int key { get; set; }

    [JsonPropertyName("parentId")]
    public int ParentId { get; set;  }

    [JsonIgnore]
    public string WebUri
        => $"https://wiki.kurobbs.com/mc/catalogue/list?fid={ParentId}&sid={key}";

}