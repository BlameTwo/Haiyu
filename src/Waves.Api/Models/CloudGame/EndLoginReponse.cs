﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.CloudGame
{
    public class EndLoginReponseData
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

    public class EndLoginReponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public EndLoginReponseData Data { get; set; }
    }
}
