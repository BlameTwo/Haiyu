using System.Text.Json.Serialization;

namespace Waves.Api.Models.Communitys.DataCenter.ResourceBrief;

public class CoinList
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("num")]
    public int Num { get; set; }

    [JsonPropertyName("sort")]
    public int Sort { get; set; }
}

public class ResourceBrefItemData
{
    [JsonPropertyName("totalCoin")]
    public int TotalCoin { get; set; }

    [JsonPropertyName("totalStar")]
    public int TotalStar { get; set; }

    [JsonPropertyName("coinList")]
    public List<CoinList> CoinList { get; set; }

    [JsonPropertyName("starList")]
    public List<StarList> StarList { get; set; }

    [JsonPropertyName("coinInc")]
    public object CoinInc { get; set; }

    [JsonPropertyName("starInc")]
    public object StarInc { get; set; }

    [JsonPropertyName("copyWriting")]
    public string CopyWriting { get; set; }

    [JsonPropertyName("recommend")]
    public Recommend Recommend { get; set; }
}

public class Recommend
{
    [JsonPropertyName("postId")]
    public string PostId { get; set; }

    [JsonPropertyName("postTitle")]
    public string PostTitle { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("picUrl")]
    public string PicUrl { get; set; }
}

public class ResourceBrefItem
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("data")]
    public ResourceBrefItemData Data { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}

public class StarList
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("num")]
    public int Num { get; set; }

    [JsonPropertyName("sort")]
    public int Sort { get; set; }
}
