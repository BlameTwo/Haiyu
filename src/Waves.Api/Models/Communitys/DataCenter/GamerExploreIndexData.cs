using System.Text.Json.Serialization;

namespace Waves.Api.Models.Communitys.DataCenter;

public class AreaInfoList
{
    [JsonPropertyName("areaId")]
    public int AreaId { get; set; }

    [JsonPropertyName("areaName")]
    public string AreaName { get; set; }

    [JsonPropertyName("areaProgress")]
    public int AreaProgress { get; set; }

    [JsonPropertyName("itemList")]
    public List<ItemList> ItemList { get; set; }
}

public class Country
{
    [JsonPropertyName("bgColor")]
    public string BgColor { get; set; }

    [JsonPropertyName("countryId")]
    public int CountryId { get; set; }

    [JsonPropertyName("countryName")]
    public string CountryName { get; set; }

    [JsonPropertyName("detailPageAreaMaskColor")]
    public string DetailPageAreaMaskColor { get; set; }

    [JsonPropertyName("detailPageAreaPic")]
    public string DetailPageAreaPic { get; set; }

    [JsonPropertyName("detailPageDarkColor")]
    public string DetailPageDarkColor { get; set; }

    [JsonPropertyName("detailPageFontColor")]
    public string DetailPageFontColor { get; set; }

    [JsonPropertyName("detailPageImage")]
    public string DetailPageImage { get; set; }

    [JsonPropertyName("detailPageLightColor")]
    public string DetailPageLightColor { get; set; }

    [JsonPropertyName("detailPagePic")]
    public string DetailPagePic { get; set; }

    [JsonPropertyName("detailPageProgressColor")]
    public string DetailPageProgressColor { get; set; }

    [JsonPropertyName("homePageIcon")]
    public string HomePageIcon { get; set; }

    [JsonPropertyName("homePageImage")]
    public string HomePageImage { get; set; }
}

public class DetectionInfoList
{
    [JsonPropertyName("acronym")]
    public string Acronym { get; set; }

    [JsonPropertyName("detectionIcon")]
    public string DetectionIcon { get; set; }

    [JsonPropertyName("detectionId")]
    public int DetectionId { get; set; }

    [JsonPropertyName("detectionName")]
    public string DetectionName { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("levelName")]
    public string LevelName { get; set; }
}

public class ExploreList
{
    [JsonPropertyName("areaInfoList")]
    public List<AreaInfoList> AreaInfoList { get; set; }

    [JsonPropertyName("country")]
    public Country Country { get; set; }

    [JsonPropertyName("countryProgress")]
    public string CountryProgress { get; set; }
}

public class ItemList
{
    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("progress")]
    public int Progress { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }
}

public class GamerExploreIndexData
{
    [JsonPropertyName("detectionInfoList")]
    public List<DetectionInfoList> DetectionInfoList { get; set; }

    [JsonPropertyName("exploreList")]
    public List<ExploreList> ExploreList { get; set; }

    [JsonPropertyName("open")]
    public bool Open { get; set; }
}
