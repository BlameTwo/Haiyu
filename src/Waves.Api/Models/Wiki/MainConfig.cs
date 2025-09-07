using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.Wiki
{
    public class Announcement
    {
        [JsonPropertyName("linkCardVisible")]
        public bool LinkCardVisible { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("linkCard")]
        public LinkCard LinkCard { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class Background
    {
        [JsonPropertyName("x")]
        public string X { get; set; }

        [JsonPropertyName("y")]
        public string Y { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Banner
    {
        [JsonPropertyName("linkConfig")]
        public LinkConfig LinkConfig { get; set; }

        [JsonPropertyName("dateRange")]
        public List<string> DateRange { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("describe")]
        public string Describe { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class WikiContent
    {
        [JsonPropertyName("linkConfig")]
        public LinkConfig LinkConfig { get; set; }

        [JsonPropertyName("contentUrl")]
        public string ContentUrl { get; set; }

        [JsonPropertyName("isNewest")]
        public bool IsNewest { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("imageNameMap")]
        public object ImageNameMap { get; set; }

        [JsonPropertyName("mobileImgUrl")]
        public string MobileImgUrl { get; set; }
    }

    public class ContentJson
    {
        [JsonPropertyName("feedback")]
        public List<Feedback> Feedback { get; set; }

        [JsonPropertyName("background")]
        public Background Background { get; set; }

        [JsonPropertyName("mainModules")]
        public List<MainModule> MainModules { get; set; }

        [JsonPropertyName("shortcuts")]
        public Shortcuts Shortcuts { get; set; }

        [JsonPropertyName("banner")]
        public List<Banner> Banner { get; set; }

        [JsonPropertyName("sideModules")]
        public List<SideModule> SideModules { get; set; }

        [JsonPropertyName("announcement")]
        public List<Announcement> Announcement { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sort")]
        public object Sort { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("checkStatus")]
        public int CheckStatus { get; set; }

        [JsonPropertyName("version")]
        public object Version { get; set; }

        [JsonPropertyName("onlineVersion")]
        public object OnlineVersion { get; set; }

        [JsonPropertyName("contentJson")]
        public ContentJson ContentJson { get; set; }
    }

    public class Feedback
    {
        [JsonPropertyName("linkConfig")]
        public LinkConfig LinkConfig { get; set; }

        [JsonPropertyName("contentUrl")]
        public string ContentUrl { get; set; }

        [JsonPropertyName("contentUrlRealName")]
        public string ContentUrlRealName { get; set; }

        [JsonPropertyName("disableLinkConfig")]
        public bool DisableLinkConfig { get; set; }

        [JsonPropertyName("fixed")]
        public bool Fixed { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }


    public class LinkCard
    {
        [JsonPropertyName("imgUrl")]
        public string ImgUrl { get; set; }

        [JsonPropertyName("linkConfig")]
        public LinkConfig LinkConfig { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class LinkConfig
    {
        [JsonPropertyName("linkUrl")]
        public string LinkUrl { get; set; }

        [JsonPropertyName("linkType")]
        public int LinkType { get; set; }

        [JsonPropertyName("catalogueId")]
        public int CatalogueId { get; set; }

        [JsonPropertyName("entryId")]
        public string EntryId { get; set; }
    }

    public class MainModule
    {
        [JsonPropertyName("toolslayerVisible")]
        public bool ToolslayerVisible { get; set; }

        [JsonPropertyName("more")]
        public WIkiMore More { get; set; }

        [JsonPropertyName("editLayerHide")]
        public bool EditLayerHide { get; set; }

        [JsonPropertyName("asideLineVisible")]
        public bool AsideLineVisible { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public object Content { get; set; }

        [JsonPropertyName("unique")]
        public bool? Unique { get; set; }
    }

    public class WIkiMore
    {
        [JsonPropertyName("linkConfig")]
        public LinkConfig LinkConfig { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class MainConfig
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Shortcuts
    {
        [JsonPropertyName("toolslayerVisible")]
        public bool ToolslayerVisible { get; set; }

        [JsonPropertyName("more")]
        public WIkiMore More { get; set; }

        [JsonPropertyName("editLayerHide")]
        public bool EditLayerHide { get; set; }

        [JsonPropertyName("asideLineVisible")]
        public bool AsideLineVisible { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public List<WikiContent> Content { get; set; }
    }

    public class SideModule
    {
        [JsonPropertyName("toolslayerVisible")]
        public bool ToolslayerVisible { get; set; }

        [JsonPropertyName("more")]
        public WIkiMore More { get; set; }

        [JsonPropertyName("unique")]
        public bool Unique { get; set; }

        [JsonPropertyName("editLayerHide")]
        public bool EditLayerHide { get; set; }

        [JsonPropertyName("asideLineVisible")]
        public bool AsideLineVisible { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public object Content { get; set; }
    }


}
