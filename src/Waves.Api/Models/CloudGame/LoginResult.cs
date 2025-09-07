﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.CloudGame;

public class LoginData
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("sdkuserid")]
    public string Sdkuserid { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("loginType")]
    public int LoginType { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("idStat")]
    public int IdStat { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("cuid")]
    public string Cuid { get; set; }

    [JsonPropertyName("showPaw")]
    public bool ShowPaw { get; set; }

    [JsonPropertyName("bindDevStat")]
    public int BindDevStat { get; set; }

    [JsonPropertyName("autoToken")]
    public string AutoToken { get; set; }

    [JsonPropertyName("autoTokenStatus")]
    public bool AutoTokenStatus { get; set; }

    [JsonPropertyName("firstLgn")]
    public int FirstLgn { get; set; }

    [JsonPropertyName("phoneCheck")]
    public int PhoneCheck { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("phoneToken")]
    public string PhoneToken { get; set; }
}

public class LoginResult
{
    [JsonPropertyName("data")]
    public LoginData Data { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}
