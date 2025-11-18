using System.Text.Json.Serialization;

namespace Haiyu.Models.ColorGames;

[JsonSerializable(typeof(ColorInfo))]
public sealed partial class GameContext : JsonSerializerContext
{
}
