using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.CloudGame
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class EndLoginData
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("uniqueId")]
        public string UniqueId { get; set; }

        [JsonPropertyName("walletData")]
        public WalletData WalletData { get; set; }

        [JsonPropertyName("hsstsToken")]
        public HsstsToken HsstsToken { get; set; }
    }

    public class ExperienceCardInfo
    {
        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("hour")]
        public int Hour { get; set; }

        [JsonPropertyName("minute")]
        public int Minute { get; set; }

        [JsonPropertyName("second")]
        public int Second { get; set; }
    }

    public class FreeTimeInfo
    {
        [JsonPropertyName("leftSeconds")]
        public int LeftSeconds { get; set; }
    }

    public class HsstsToken
    {
        [JsonPropertyName("ak")]
        public string Ak { get; set; }

        [JsonPropertyName("sk")]
        public string Sk { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    public class PayTimeInfo
    {
        [JsonPropertyName("leftSeconds")]
        public int LeftSeconds { get; set; }
    }

    public class EndLoginModel
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public EndLoginData Data { get; set; }
    }

    public class TimeCardInfo
    {
        [JsonPropertyName("expireTimeSeconds")]
        public int ExpireTimeSeconds { get; set; }
    }

    public class WalletData
    {
        [JsonPropertyName("freeTimeInfo")]
        public FreeTimeInfo FreeTimeInfo { get; set; }

        [JsonPropertyName("payTimeInfo")]
        public PayTimeInfo PayTimeInfo { get; set; }

        [JsonPropertyName("timeCardInfo")]
        public TimeCardInfo TimeCardInfo { get; set; }

        [JsonPropertyName("experienceCardInfo")]
        public ExperienceCardInfo ExperienceCardInfo { get; set; }

        [JsonPropertyName("coin")]
        public int Coin { get; set; }
    }
}
