using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Waves.Api.Models.Messanger;

namespace Waves.Api.Models.Communitys;

public class GamerRoil
{
    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// 请求成功
    /// </summary>
    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("data")]
    public List<GameRoilDataItem> Data { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}

public class GameRoilDataItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("gameId")]
    public int GameId { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("serverId")]
    public string ServerId { get; set; }

    /// <summary>
    /// 鸣潮
    /// </summary>
    [JsonPropertyName("serverName")]
    public string ServerName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("roleId")]
    public string RoleId { get; set; }

    /// <summary>
    /// 怪
    /// </summary>
    [JsonPropertyName("roleName")]
    public string RoleName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("gameHeadUrl")]
    public string GameHeadUrl { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("gameLevel")]
    public string GameLevel { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("roleScore")]
    public string RoleScore { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("roleNum")]
    public int RoleNum { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("fashionCollectionPercent")]
    public int FashionCollectionPercent { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("phantomPercent")]
    public double PhantomPercent { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("achievementCount")]
    public long AchievementCount { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("actionRecoverSwitch")]
    public bool ActionRecoverSwitch { get; set; }

    [JsonIgnore]
    public IRelayCommand CopyIdCommand =>
        new RelayCommand(() =>
        {
            WeakReferenceMessenger.Default.Send<CopyStringMessager>(new(this.RoleId));
        });
}
