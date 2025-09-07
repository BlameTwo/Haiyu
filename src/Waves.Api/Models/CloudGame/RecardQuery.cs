using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.CloudGame
{
    public class RecardQuery
    {
        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        [JsonPropertyName("cardPoolId")]
        public string CardPoolId { get; set; }

        [JsonPropertyName("cardPoolType")]
        public int CardPoolType { get; set; }

        [JsonPropertyName("serverId")]
        public string ServerId { get; set; }

        [JsonPropertyName("languageCode")]
        public string LanguageCode { get; set; }

        [JsonPropertyName("recordId")]
        public string RecordId { get; set; }
    }
}
