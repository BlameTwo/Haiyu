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
    public partial class WikiContext:JsonSerializerContext
    {
    }
}
