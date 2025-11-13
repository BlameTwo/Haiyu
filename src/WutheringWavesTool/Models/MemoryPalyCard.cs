using MemoryPack;
using Waves.Api.Models.CloudGame;

namespace Haiyu.Models;

[MemoryPackable]
public partial class MemoryPalyCard
{
    public string UserName { get; set; }

    public List<Datum> Values { get; set; }
}
