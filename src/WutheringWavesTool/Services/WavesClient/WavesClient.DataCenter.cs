﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Waves.Api.Models;
using Waves.Api.Models.Communitys;
using Waves.Api.Models.Communitys.DataCenter;

namespace WutheringWavesTool.Services;

partial class WavesClient
{
    public async Task<GamerBassData?> GetGamerBassDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
            { "channelId", "19" },
            { "countryCode", "1" },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/gamer/roleBox/akiBox/baseData",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);

        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200)
        {
            return null;
        }
        var bassData = JsonSerializer.Deserialize(
            resultCode.Data,
            CommunityContext.Default.GamerBassData
        );
        return bassData;
    }

    public async Task<GamerRoleData?> GetGamerRoleDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
            { "channelId", "19" },
            { "countryCode", "1" },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/aki/roleBox/akiBox/roleData",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200)
        {
            return null;
        }
        var jsonData = resultCode.Data;
        return JsonSerializer.Deserialize(jsonData, CommunityContext.Default.GamerRoleData);
    }

    public async Task<GamerCalabashData?> GetGamerCalabashDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
            { "channelId", "19" },
            { "countryCode", "1" },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/gamer/roleBox/akiBox/calabashData",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200)
        {
            return null;
        }
        var jsonData = resultCode.Data;

        return JsonSerializer.Deserialize(jsonData, CommunityContext.Default.GamerCalabashData);
    }

    public async Task<GamerTowerModel?> GetGamerTowerIndexDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/aki/roleBox/akiBox/towerDataDetail",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200)
        {
            return null;
        }
        var jsonData = resultCode.Data;
        return JsonSerializer.Deserialize(jsonData, CommunityContext.Default.GamerTowerModel);
    }

    public async Task<GamerExploreIndexData?> GetGamerExploreIndexDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/aki/roleBox/akiBox/exploreIndex",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200)
        {
            return null;
        }
        var jsonData = resultCode.Data;

        return JsonSerializer.Deserialize(jsonData, CommunityContext.Default.GamerExploreIndexData);
    }

    public async Task<GamerChallengeIndexData?> GetGamerChallengeIndexDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/aki/roleBox/akiBox/challengeIndex",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200)
        {
            return null;
        }
        var jsonData = resultCode.Data;
        return JsonSerializer.Deserialize(
            jsonData,
            CommunityContext.Default.GamerChallengeIndexData
        );
    }

    public async Task<GamerDataBool?> RefreshGamerDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
            { "channelId", "19" },
            { "countryCode", "1" },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/aki/roleBox/akiBox/refreshData",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);

        return JsonSerializer.Deserialize(jsonStr, CommunityContext.Default.GamerDataBool);
    }

    public async Task<GamerRoilDetily?> GetGamerRoilDetily(
        GameRoilDataItem roil,
        long roleId,
        CancellationToken token = default
    )
    {
        //https://api.kurobbs.com/aki/roleBox/akiBox/getRoleDetail
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId },
            { "serverId", roil.ServerId },
            { "channelId", "19" },
            { "countryCode", "1" },
            { "id", $"{roleId}" },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/aki/roleBox/akiBox/getRoleDetail",
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200) { }
        var jsonData = resultCode.Data;

        return JsonSerializer.Deserialize(jsonData, CommunityContext.Default.GamerRoilDetily);
    }

    public async Task<GamerChallengeDetily?> GetGamerChallengeDetails(
        GameRoilDataItem roil,
        int countryCode,
        CancellationToken token = default
    )
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>()
        {
            { "gameId", roil.GameId.ToString() },
            { "roleId", roil.RoleId.ToString() },
            { "serverId", roil.ServerId.ToString() },
            { "channelId", "19" },
            { "countryCode", countryCode.ToString() },
        };
        var request = await BuildRequest(
            "https://api.kurobbs.com/aki/roleBox/akiBox/challengeDetails",
            HttpMethod.Post,
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            content
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null || resultCode.Code != 200)
        {
            return null;
        }
        var jsonData = resultCode.Data;
        var result2 = JsonSerializer.Deserialize(
            jsonData,
            CommunityContext.Default.GamerChallengeDetily
        );
        return result2;
    }
}
