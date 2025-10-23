using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.GameWikiiClient
{
    public class EventContentSide
    {
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("tabs")]
        public List<EventSideTab> Tabs { get; set; }
    }

    public class EventSideTab
    {
        [JsonPropertyName("countDown")]
        public CountDown CountDown { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("imgs")]
        public List<EventSideImage> Images { get; set; }
    }

    public class EventSideImage
    {
        [JsonPropertyName("img")]
        public string Image { get; set; }
    }
}
