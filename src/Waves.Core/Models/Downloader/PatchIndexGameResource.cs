using System.Text.Json.Serialization;

namespace Waves.Core.Models.Downloader;

public class Entry
{
    [JsonPropertyName("dest")]
    public string Dest { get; set; }

    [JsonPropertyName("md5")]
    public string Md5 { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("chunkInfos")]
    public List<IndexChunkInfo> ChunkInfos { get; set; }
}

public class PatchInfo
{
    [JsonPropertyName("dest")]
    public string Dest { get; set; }

    [JsonPropertyName("entries")]
    public List<IndexResource> Entries { get; set; }
}

public class PatchResource
{
    [JsonPropertyName("dest")]
    public string Dest { get; set; }

    [JsonPropertyName("md5")]
    public string Md5 { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("fromFolder")]
    public string FromFolder { get; set; }

    [JsonPropertyName("chunkInfos")]
    public List<IndexChunkInfo> ChunkInfos { get; set; }
}


public class PatchIndexGameResource : IndexGameResource
{
    [JsonPropertyName("deleteFiles")]
    public List<string> DeleteFiles { get; set; }

    [JsonPropertyName("applyTypes")]
    public List<string> ApplyTypes { get; set; }

    [JsonPropertyName("patchInfos")]
    public List<PatchInfo> PatchInfos { get; set; }
}

[JsonSerializable(typeof(PatchInfo))]
[JsonSerializable(typeof(Entry))]
[JsonSerializable(typeof(PatchResource))]
[JsonSerializable(typeof(PatchIndexGameResource))]
public partial class PathIndexGameResourceContext : JsonSerializerContext { }
