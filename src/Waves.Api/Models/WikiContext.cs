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
    [JsonSerializable(typeof(SideEventData))]
    [JsonSerializable(typeof(List<SideEventData>))]
    public partial class WikiContext:JsonSerializerContext
    {
    }
}
