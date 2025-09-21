using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.QRLogin
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        [JsonPropertyName("autoLogin")]
        public bool AutoLogin { get; set; }

        [JsonPropertyName("bindTimeStamp")]
        public string BindTimeStamp { get; set; }

        [JsonPropertyName("channelId")]
        public string ChannelId { get; set; }

        [JsonPropertyName("cuid")]
        public string Cuid { get; set; }

        [JsonPropertyName("gameId")]
        public int GameId { get; set; }

        [JsonPropertyName("gameName")]
        public string GameName { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("mode")]
        public int Mode { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("roleId")]
        public string RoleId { get; set; }

        [JsonPropertyName("roleName")]
        public string RoleName { get; set; }

        [JsonPropertyName("serverId")]
        public string ServerId { get; set; }

        [JsonPropertyName("serverName")]
        public string ServerName { get; set; }

        [JsonPropertyName("support")]
        public bool Support { get; set; }

        [JsonIgnore]
        public string GetName
        {
            get
            {
                if(ServerName == null)
                {
                    return $"{this.GameName}({this.Mobile})";
                }
                return $"{this.ServerName}({this.RoleName})";
            }
        }
    }

    public class ScanScreenModel
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public List<Datum> Data { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }


}
