using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Waves.Api.Models.Wiki;

namespace Waves.Api.Models
{
    [JsonSerializable(typeof(Announcement))]
    [JsonSerializable(typeof(Background))]
    [JsonSerializable(typeof(Banner))]
    [JsonSerializable(typeof(WikiContent))]
    [JsonSerializable(typeof(ContentJson))]
    [JsonSerializable(typeof(Data))]
    [JsonSerializable(typeof(Feedback))]
    [JsonSerializable(typeof(LinkCard))]
    [JsonSerializable(typeof(LinkConfig))]
    [JsonSerializable(typeof(MainModule))]
    [JsonSerializable(typeof(WIkiMore))]
    [JsonSerializable(typeof(MainConfig))]
    [JsonSerializable(typeof(Shortcuts))]
    [JsonSerializable(typeof(SideModule))]
    [JsonSerializable(typeof(CountDown))]
    [JsonSerializable(typeof(EventSideDataRange))]
    [JsonSerializable(typeof(SideImg))]
    [JsonSerializable(typeof(InnerTab))]
    [JsonSerializable(typeof(Repeat))]
    [JsonSerializable(typeof(EventsSide))]
    [JsonSerializable(typeof(Tab))]
    public partial class WikiContext:JsonSerializerContext
    {
    }
}
