using System.Text.Json.Serialization;

namespace Waves.Api.Models.Communitys.DataCenter.ResourceBrief;
public class BrieData
{
    [JsonPropertyName("weeks")]
    public List<BrieWeek> Weeks { get; set; }

    [JsonPropertyName("months")]
    public List<BrieMonth> Months { get; set; }

    [JsonPropertyName("versions")]
    public List<BrieVersion> Versions { get; set; }
}

public class BrieMonth
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }
}

public class BriefHeader
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("data")]
    public BrieData Data { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}

public class BrieVersion
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }
}

public class BrieWeek
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }
}


