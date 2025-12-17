using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Waves.Api.Models.GameWikiiClient;

namespace Waves.Api.Models
{
    [JsonSerializable(typeof(WikiHomeModel))]
    [JsonSerializable(typeof(HotContentSide))]
    [JsonSerializable(typeof(List<HotContentSide>))]
    [JsonSerializable(typeof(List<EventContentSide>))]
    [JsonSerializable(typeof(EventContentSide))]
    [JsonSerializable(typeof(EventSideTab))]
    [JsonSerializable(typeof(EventSideImage))]
    [JsonSerializable(typeof(WikiCatalogue))]
    [JsonSerializable(typeof(WikiCatalogueChildren))]
    [JsonSerializable(typeof(WeekContentJsonContent))]
    [JsonSerializable(typeof(List<WeekContentJsonContent>))]
    [JsonSerializable(typeof(WeekContentTab))]
    [JsonSerializable(typeof(List<WeekContentTab>))]
    [JsonSerializable(typeof(WeekContentImg))]
    [JsonSerializable(typeof(WeekContentInnerTab))]
    [JsonSerializable(typeof(WeekContentCountDown))]
    [JsonSerializable(typeof(WeekContentRepeat))]
    [JsonSerializable(typeof(WeekContentDataRange))]
    [JsonSerializable(typeof(List<DisputeJsonItem>))]
    public partial class WikiContext : JsonSerializerContext
    {
    }
}
