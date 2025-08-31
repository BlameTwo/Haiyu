﻿using Waves.Api.Models.Communitys.DataCenter.ResourceBrief;
using Waves.Api.Models.QRLogin;

namespace WavesLauncher.Core.Contracts;

public interface IWavesClient
{
    public string Token { get; }

    public long Id { get; }

    public Task<bool> IsLoginAsync(CancellationToken token = default);

    public IHttpClientService HttpClientService { get; }

    public Task<GamerDataModel?> GetGamerDataAsync(
        GameRoilDataItem gamerRoil,
        CancellationToken token = default
    );
    public Task<GamerRoil?> GetWavesGamerAsync(CancellationToken token = default);

    public Task<SMSResultModel?> SendSMSAsync(
        string mobile,
        string geeTestData,
        CancellationToken token = default
    );

    public Task<SignIn?> GetSignInDataAsync(string userId, long roleId);

    public Task<AccountModel?> LoginAsync(
        string mobile,
        string code,
        CancellationToken token = default
    );

    public Task<SignRecord?> GetSignRecordAsync(string userId, string roldId);
    public Task<SignInResult?> SignInAsync(string userId, string roleId,CancellationToken token = default);
    public Task<AccountMine?> GetWavesMineAsync(long id, CancellationToken token = default);

    public Task<PlayerReponse?> GetPlayerReponseAsync(PlayerCard card);
    public Task<ScanScreenModel?> PostQrValueAsync(string qrText, CancellationToken token = default);

    public Task<QRLoginResult?> QRLoginAsync(string qrText, string verifyCode, CancellationToken token = default);

    public Task<SMSModel?> GetQrCodeAsync(string qrCode, CancellationToken token = default);

    public Task<DeviceInfo?> GetDeviceInfosAsync(CancellationToken token = default); 
    public Task<AddUserGameServer?> GetBindServerAsync(int gameId, CancellationToken token = default);

    public Task<SendGameVerifyCode?> SendVerifyGameCode(string gameId, string serverId,string roldId, CancellationToken token = default);

    public Task<BindGameVerifyCode?> BindGamer(string gameId, string serverId, string roleId, string verifyCode, CancellationToken token = default);
    public GameRoilDataWrapper CurrentRoil { get; set; }

    #region 数据终端
    Task<GamerBassData?> GetGamerBassDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );
    Task<GamerRoleData?> GetGamerRoleDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );

    Task<GamerCalabashData?> GetGamerCalabashDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );

    Task<GamerTowerModel?> GetGamerTowerIndexDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );

    Task<GamerExploreIndexData?> GetGamerExploreIndexDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );

    Task<GamerChallengeIndexData?> GetGamerChallengeIndexDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );

    Task<GamerDataBool?> RefreshGamerDataAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );

    Task<GamerRoilDetily?> GetGamerRoilDetily(
        GameRoilDataItem roil,
        long roleId,
        CancellationToken token = default
    );

    Task<GamerChallengeDetily?> GetGamerChallengeDetails(
        GameRoilDataItem roil,
        int countryCode,
        CancellationToken token = default
    );

    Task<GamerSkin?> GetGamerSkinAsync(GameRoilDataItem roil, CancellationToken token = default);

    public Task<GamerSlashDetailData?> GetGamerSlashDetailAsync(
        GameRoilDataItem roil,
        CancellationToken token = default
    );

    Task<BriefHeader?> GetBriefHeaderAsync(CancellationToken token = default);

    public Task<ResourceBrefItem> GetVersionBrefItemAsync(string roleId, string serverId, string versionId, CancellationToken token = default);
    public Task<ResourceBrefItem> GetWeekBrefItemAsync(string roleId, string serverId, string versionId, CancellationToken token = default);
    public Task<ResourceBrefItem> GetMonthBrefItemAsync(string roleId, string serverId, string versionId, CancellationToken token = default);
    #endregion
    public Task<RefreshToken?> UpdateRefreshToken(
        GameRoilDataItem item,
        CancellationToken token = default
    );

    public Task InitAsync();
}
