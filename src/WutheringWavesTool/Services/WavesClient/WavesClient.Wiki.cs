﻿using System.Net.Http.Json;
using Waves.Api.Models.Wiki;
using WutheringWavesTool.Helpers;

namespace WutheringWavesTool.Services;

partial class WavesClient
{
    public async Task<MainConfig> GetMainWikiAsync(CancellationToken token = default)
    {
        var header = new Dictionary<string, string>()
        {
            { "devcode", HardwareIdGenerator.GenerateUniqueId() },
            { "wiki_type", "9" },
        };
        var request = await BuildRequestAsync(
            "https://api.kurobbs.com/wiki/core/homepage/getPage",
            HttpMethod.Post,
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded", "UTF-8"),
            [],
            false,
            token
        );
        var reponse = await HttpClientService.HttpClient.SendAsync(request,token);
        return await reponse.Content.ReadFromJsonAsync(WikiContext.Default.MainConfig, token);
    }
}
