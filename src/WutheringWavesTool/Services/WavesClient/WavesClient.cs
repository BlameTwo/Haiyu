using Azure.Core;
using NetTaste;
using Waves.Api.Models.QRLogin;
using Windows.System.Profile;
using ZXing.Aztec.Internal;

namespace WutheringWavesTool.Services;

public sealed partial class WavesClient : IWavesClient
{
    public string Token => AppSettings.Token ?? "";

    public long Id => long.Parse(AppSettings.TokenId ?? "0");

    public IHttpClientService HttpClientService { get; }
    public GameRoilDataWrapper CurrentRoil { get; set; }
    public string? BAT { get; private set; }

    public WavesClient(IHttpClientService httpClientService)
    {
        HttpClientService = httpClientService;
        HttpClientService.BuildClient();
    }
    public string GetPackageSpecificId()
    {
        try
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            var hardwareId = token.Id;
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);
            byte[] bytes = new byte[hardwareId.Length];
            dataReader.ReadBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", "");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return string.Empty;
        }
    }
    

    private Dictionary<string, string> GetHeader(bool isNeedToken, bool isNeedBAT = true)
    {
        var dict = new Dictionary<string, string>()
        {
            { "Accept", "application/json, text/plain, */*" },
            { "Accept-Encoding", "gzip, deflate" },
            { "Accept-Language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7" },
            {
                "User-Agent",
                "Mozilla/5.0 (Linux; Android 9; SM-S9260 Build/PQ3A.190705.01061653; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/91.0.4472.114 Safari/537.36 Kuro/2.5.0 KuroGameBox/2.5.0"
            },
            { "source", "android" },
            { "did", "1F48CAEBD509B31B7D63AACFE543FCA356AB50D8" },
            {
                "devCode",
                "AAB886CD651C77F028B4E7A883D10A1240E08BBF"
                //GetPackageSpecificId()
            },
            { "model","SM-A5260"},
            { "version","2.5.3"},
            { "lang","zh-Hans"},
            { "countryCode","CN"},
        };
        if (isNeedBAT)
        {
            if (!string.IsNullOrWhiteSpace(this.BAT))
                dict.Add("b-at", this.BAT ?? "");
        }
        if (isNeedToken)
        {
            if (this.Token == null || string.IsNullOrWhiteSpace(Token))
            {
                dict.Add("token", this.Token);
            }
            else
            {
                dict.Add("token", Token);
            }
        }
        return dict;
    }

    private async Task<HttpRequestMessage> BuildLoginRequest(
        string url,
        Dictionary<string, string> headers,
        MediaTypeHeaderValue mediatype,
        Dictionary<string, string> queryValues,
        CancellationToken token = default
    )
    {
        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;
        foreach (var item in headers)
        {
            request.Headers.Add(item.Key, item.Value);
        }
        request.RequestUri = new Uri(url);

        var endcod = new FormUrlEncodedContent(queryValues);
        var query = await endcod.ReadAsStringAsync(token);
        request.Content = new StringContent(query, mediatype);
        return request;
    }

    private async Task<HttpRequestMessage> BuildRequestAsync(
        string url,
        HttpMethod method,
        Dictionary<string, string> headers,
        MediaTypeHeaderValue mediatype,
        Dictionary<string, string> queryValues,
        bool IsNeedToken = false,
        CancellationToken token = default
    )
    {
        var request = new HttpRequestMessage();
        request.Method = method;
        foreach (var item in headers)
        {
            request.Headers.Add(item.Key, item.Value);
        }
        request.RequestUri = new Uri(url);
        var endcod = new FormUrlEncodedContent(queryValues);
        var query = await endcod.ReadAsStringAsync(token);
        request.Content = new StringContent(query, mediatype);
        return request;
    }

    public async Task<SignIn?> GetSignInDataAsync(string userId, long roleId)
    {
        var queryData = new Dictionary<string, string>()
        {
            { "gameId", "3" },
            { "serverId", "76402e5b20be2c39f095a152090afddc" },
            { "roleId", roleId.ToString() },
            { "userId", userId.ToString() },
        };
        var header = GetHeader(true);
        var request = await BuildRequestAsync(
            "https://api.kurobbs.com/encourage/signIn/initSignInV2",
            HttpMethod.Post,
            header,
            new("application/x-www-form-urlencoded"),
            queryData,
            true
        );
        var result = await HttpClientService.HttpClient.SendAsync(request);
        var jsonStr = await result.Content.ReadAsStringAsync();
        var sign = JsonSerializer.Deserialize(jsonStr, CommunityContext.Default.SignIn);
        return sign;
    }

    public async Task<SignRecord?> GetSignRecordAsync(string userId, string roldId)
    {
        var header = GetHeader(true);
        var queryData = new Dictionary<string, string>()
        {
            { "gameId", "3" },
            { "serverId", "76402e5b20be2c39f095a152090afddc" },
            { "userId", userId },
            { "roleId", roldId },
            { "reqMonth", DateTime.Now.Month.ToString("D2") },
        };
        var request = await BuildRequestAsync(
            "https://api.kurobbs.com/encourage/signIn/queryRecordV2",
            HttpMethod.Post,
            header,
            new("application/x-www-form-urlencoded"),
            queryData,
            true
        );
        var result = await HttpClientService.HttpClient.SendAsync(request);
        string jsonStr = await result.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize(jsonStr, CommunityContext.Default.SignRecord);
    }

    public async Task<SignInResult?> SignInAsync(string userId, string roleId)
    {
        var header = GetHeader(true, false);
        var queryData = new Dictionary<string, string>()
        {
            { "gameId", "3" },
            { "serverId", "76402e5b20be2c39f095a152090afddc" },
            { "userId", userId },
            { "roleId", roleId },
            { "reqMonth", DateTime.Now.Month.ToString("D2") },
        };
        var request = await BuildRequestAsync(
            "https://api.kurobbs.com/encourage/signIn/v2",
            HttpMethod.Post,
            header,
            new("application/x-www-form-urlencoded"),
            queryData,
            true
        );
        var result = await HttpClientService.HttpClient.SendAsync(request);
        result.EnsureSuccessStatusCode();
        string jsonStr = await result.Content.ReadAsStringAsync();
        var jsonObj = JsonObject.Parse(jsonStr);
        if (jsonObj["code"]!.GetValue<int>() != 200) { }
        return JsonSerializer.Deserialize(jsonStr, CommunityContext.Default.SignInResult);
    }

    public async Task<AccountMine?> GetWavesMineAsync(long id, CancellationToken token = default)
    {
        var header = GetHeader(true);
        var content = new Dictionary<string, string>() { { "otherUserId", id.ToString() } };
        var request = await BuildRequestAsync(
            "https://api.kurobbs.com/user/mineV2",
            HttpMethod.Post,
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded", "utf-8"),
            content,
            true,
            token
        );
        var result = await HttpClientService.HttpClient.SendAsync(request);
        var jsonStr = await result.Content.ReadAsStringAsync();
        return (AccountMine?)
            JsonSerializer.Deserialize(jsonStr, typeof(AccountMine), CommunityContext.Default);
    }

    public async Task<PlayerReponse?> GetPlayerReponseAsync(PlayerCard card)
    {
        try
        {
            var url = "https://gmserver-api.aki-game2.com/gacha/record/query";
            var content = new StringContent(
                JsonSerializer.Serialize(card, CommunityContext.Default.PlayerCard),
                System.Text.Encoding.UTF8,
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
            );
            var message = new HttpRequestMessage()
            {
                Content = content,
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };
            var reponse = await HttpClientService.HttpClient.SendAsync(message);
            var Jsonstr = await reponse.Content.ReadAsStringAsync();
            return (PlayerReponse?)
                JsonSerializer.Deserialize(
                    await reponse.Content.ReadAsStringAsync(),
                    CommunityContext.Default.PlayerReponse
                );
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> IsLoginAsync(CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(Token) || Id <= 0)
        {
            return false;
        }
        var mine = await GetWavesMineAsync(Id, token);
        if (mine != null)
        {
            if (mine.Code == 200)
                return true;
        }
        return false;
    }

    public async Task<RefreshToken?> UpdateRefreshToken(
        GameRoilDataItem item,
        CancellationToken token = default
    )
    {
        var url = "https://api.kurobbs.com/aki/roleBox/requestToken";
        var header = new Dictionary<string, string>()
        {
            { "Accept", "application/json, text/plain, */*" },
            { "Accept-Encoding", "gzip, deflate" },
            { "Accept-Language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7" },
            {
                "User-Agent",
                "Mozilla/5.0 (Linux; Android 9; SM-S9260 Build/PQ3A.190705.01061653; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/91.0.4472.114 Safari/537.36 Kuro/2.5.0 KuroGameBox/2.5.0"
            },
            {
                "devCode",
                "null, Mozilla/5.0 (Linux; Android 9; SM-S9260 Build/PQ3A.190705.01061653; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/91.0.4472.114 Safari/537.36 Kuro/2.5.0 KuroGameBox/2.5.0"
            },
            { "did", "1F48CAEBD509B31B7D63AACFE543FCA356AB50D8" },
            { "source", "android" },
            { "token", this.Token },
            { "Connection", "keep-alive" },
        };
        var request = await BuildRequestAsync(
            url,
            HttpMethod.Post,
            header,
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            new Dictionary<string, string>()
            {
                { "roleId", item.RoleId.ToString() },
                { "serverId", item.ServerId },
                { "userId", item.UserId.ToString() },
            },
            true,
            token
        );
        var result = await HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);

        var resultCode = JsonSerializer.Deserialize(
            jsonStr,
            CommunityContext.Default.GamerBassString
        );
        if (resultCode == null)
        {
            return null;
        }

        var bassData = JsonSerializer.Deserialize(
            resultCode.Data,
            AccessTokenContext.Default.RefreshToken
        );
        if (bassData != null)
        {
            this.BAT = bassData.AccessToken;
        }
        return bassData;
    }
    
    public async Task<ScanScreenModel?> PostQrValueAsync(string qrText,CancellationToken token = default)
    {
        var url = "https://api.kurobbs.com/user/auth/roleInfos";
        var request = await BuildRequestAsync(url, HttpMethod.Post, GetHeader(true,false), new MediaTypeHeaderValue("application/x-www-form-urlencoded"), new Dictionary<string, string>()
        {
            { "qrCode",qrText}
        },true);
        var result = await HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        return JsonSerializer.Deserialize<ScanScreenModel>(jsonStr, QRContext.Default.ScanScreenModel);
    }

    public async Task<QRLoginResult?> QRLoginAsync(string qrText,string verifyCode,CancellationToken token = default)
    {
        var url = "https://api.kurobbs.com/user/auth/scanLogin";
        var request = await BuildRequestAsync(url, HttpMethod.Post, GetHeader(true, false), new MediaTypeHeaderValue("application/x-www-form-urlencoded"), new Dictionary<string, string>()
        {
            {"autoLogin","true" },
            { "qrCode",qrText},
            { "verifyCode",verifyCode }
        }, true);
        var result = await HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        return JsonSerializer.Deserialize<QRLoginResult>(jsonStr, QRContext.Default.QRLoginResult);
    }

    public async Task<SMSModel?> GetQrCodeAsync(string qrCode,CancellationToken token = default)
    {
        var query = new Dictionary<string, string>()
        {
            { "geeTestData", "" },
        };
        var request = await BuildLoginRequest(
            "https://api.kurobbs.com/user/sms/scanSms",
            GetHeader(true,false),
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            query
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        return (SMSModel?)
            JsonSerializer.Deserialize(jsonStr,QRContext.Default.SMSModel);
    }

    public async Task<DeviceInfo?> GetDeviceInfosAsync(CancellationToken token = default)
    {
        var url = "https://api.kurobbs.com/user/auth/device/list";
        var request = await BuildLoginRequest(
            url,
            GetHeader(true, false),
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            []
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        return (DeviceInfo?)
            JsonSerializer.Deserialize(jsonStr, QRContext.Default.DeviceInfo);
    }

    public async Task<SendGameVerifyCode?> SendVerifyGameCode(string gameId,string serverId, string roleId,CancellationToken token = default)
    {
        var url = "https://api.kurobbs.com/user/role/sendVerifyCode";
        var request = await BuildLoginRequest(
            url,
            GetHeader(true, false),
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            new Dictionary<string, string>()
            {
                {"gameId",gameId },
                {"roleId", roleId},
                {"serverId",serverId }
            }
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        return 
            JsonSerializer.Deserialize(jsonStr, BindGameContext.Default.SendGameVerifyCode);
    }

    public async Task<AddUserGameServer?> GetBindServerAsync(int gameId,CancellationToken token = default)
    {
        var url = "https://api.kurobbs.com/config/findGameServerList";
        var request = await BuildLoginRequest(
            url,
            GetHeader(true, false),
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            new Dictionary<string, string>()
            {
                { "gameId",gameId.ToString()}
            }
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        return 
            JsonSerializer.Deserialize(jsonStr, BindGameContext.Default.AddUserGameServer);
    }

    public async Task<BindGameVerifyCode?> BindGamer(string gameId, string serverId, string roleId,string verifyCode,CancellationToken token = default)
    {
        var url = "https://api.kurobbs.com/user/role/bindUserRole";
        var request = await BuildLoginRequest(
            url,
            GetHeader(true, false),
            new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            new Dictionary<string, string>()
            {
                {"gameId",gameId },
                {"roleId", roleId},
                {"verifyCode",verifyCode },
                {"serverId",serverId }
            }
        );
        var result = await this.HttpClientService.HttpClient.SendAsync(request, token);
        var jsonStr = await result.Content.ReadAsStringAsync(token);
        return
            JsonSerializer.Deserialize(jsonStr, BindGameContext.Default.BindGameVerifyCode);
    }
}
