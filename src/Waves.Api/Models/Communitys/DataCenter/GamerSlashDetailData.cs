using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Waves.Api.Models.Communitys.DataCenter;

public class SlashChallengeList
{
    [JsonPropertyName("challengeId")]
    public int ChallengeId { get; set; }

    [JsonPropertyName("challengeName")]
    public string ChallengeName { get; set; }

    [JsonPropertyName("halfList")]
    public ObservableCollection<HalfList> HalfList { get; set; }

    [JsonPropertyName("rank")]
    public string Rank { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }
}

public class SlashDifficultyList
{
    [JsonPropertyName("allScore")]
    public int AllScore { get; set; }

    [JsonPropertyName("challengeList")]
    public List<SlashChallengeList> ChallengeList { get; set; }

    [JsonPropertyName("detailPageBG")]
    public string DetailPageBG { get; set; }

    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }

    [JsonPropertyName("difficultyName")]
    public string DifficultyName { get; set; }

    [JsonPropertyName("homePageBG")]
    public string HomePageBG { get; set; }

    [JsonPropertyName("maxScore")]
    public int MaxScore { get; set; }

    [JsonPropertyName("teamIcon")]
    public string TeamIcon { get; set; }
}

public class HalfList
{
    [JsonPropertyName("buffDescription")]
    public string BuffDescription { get; set; }

    [JsonPropertyName("buffIcon")]
    public string BuffIcon { get; set; }

    [JsonPropertyName("buffName")]
    public string BuffName { get; set; }

    [JsonPropertyName("buffQuality")]
    public int BuffQuality { get; set; }

    [JsonPropertyName("roleList")]
    public ObservableCollection<SlashRoleList> RoleList { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }
}

public class SlashRoleList
{
    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }

    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
}

public class GamerSlashDetailData
{
    [JsonPropertyName("difficultyList")]
    public List<SlashDifficultyList> DifficultyList { get; set; }

    [JsonPropertyName("isUnlock")]
    public bool IsUnlock { get; set; }

    [JsonPropertyName("seasonEndTime")]
    public long SeasonEndTime { get; set; }
}
