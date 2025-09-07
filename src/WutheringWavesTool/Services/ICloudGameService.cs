﻿using Waves.Api.Models.CloudGame;

namespace WutheringWavesTool.Services;

/// <summary>
/// 云鸣潮接口会话
/// </summary>
public interface ICloudGameService
{
    public CloudConfigManager ConfigManager { get; }

    public Task<CloudSendSMS> GetPhoneSMSAsync(
        string phone,
        string geetestCaptchaOutput,
        string geetestPassToken,
        string geetestGenTime,
        string geetestLotNumber,
        CancellationToken token = default
    );

    public Task<LoginResult> LoginAsync(
        string phone,
        string code,
        CancellationToken token = default
    );

    Task<EndLoginReponse> GetTokenAsync(PhoneTokenData data, string token);
    Task<AccessToken> GetAccessTokenAsync(string code, CancellationToken token = default);
    Task<PhoneTokenModel> LoginPhoneTokenAsync(
        string phoneToken,
        string phone,
        CancellationToken token = default
    );
    Task<RecordModel> GetRecordAsync();
    Task<PlayerReponse> GetGameRecordResource(string recordId, string userId);
    /// <summary>
    /// 创建连接会话
    /// </summary>
    /// <param name="loginData"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<(bool,string)> OpenUserAsync(LoginData loginData,CancellationToken token = default);
}
