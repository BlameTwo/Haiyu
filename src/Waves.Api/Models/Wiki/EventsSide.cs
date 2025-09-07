using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.Wiki
{
    public class CountDown
    {
        [JsonPropertyName("dateRange")]
        public List<string> DateRange { get; set; }

        [JsonPropertyName("repeat")]
        public Repeat Repeat { get; set; }

        [JsonPropertyName("precision")]
        public string Precision { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class EventSideDataRange
    {
        [JsonPropertyName("progressType")]
        public int ProgressType { get; set; }

        [JsonPropertyName("dataRange")]
        public List<string> DataRange { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class SideImg
    {
        [JsonPropertyName("linkConfig")]
        public LinkConfig LinkConfig { get; set; }

        [JsonPropertyName("img")]
        public string Img { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class InnerTab
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }


    public class Repeat
    {
        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

        [JsonPropertyName("isNeverEnd")]
        public bool IsNeverEnd { get; set; }

        [JsonPropertyName("repeatInterval")]
        public int RepeatInterval { get; set; }

        [JsonPropertyName("dataRanges")]
        public List<EventSideDataRange> DataRanges { get; set; }
    }

    public class EventsSide
    {
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("tabs")]
        public List<Tab> Tabs { get; set; }
    }

    public class Tab
    {
        [JsonPropertyName("imgs")]
        public List<SideImg> Imgs { get; set; }

        [JsonPropertyName("innerTabs")]
        public List<InnerTab> InnerTabs { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("imgMode")]
        public string ImgMode { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("countDown")]
        public CountDown CountDown { get; set; }
    }


}
