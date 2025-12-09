using Haiyu.Contracts;
using System.Security.Cryptography;
using Waves.Api.Models.Rpc;

namespace Haiyu.Services;

public enum RpcMethodKey
{
    ping,
    rpcVersion,
    getCloudUsers,
    getCloudRecordKey
}

public partial class RpcMethodService : IRpcMethodService
{
    public RpcMethodService(IKuroClient kuroClient, ICloudGameService cloudGameService, CloudConfigManager cloudConfigManager)
    {
        KuroClient = kuroClient;
        CloudGameService = cloudGameService;
        CloudConfigManager = cloudConfigManager;
    }

    public IKuroClient KuroClient { get; }
    public ICloudGameService CloudGameService { get; }
    public CloudConfigManager CloudConfigManager { get; }

    public Dictionary<string, Func<string, List<RpcParams>?, Task<string>>> Method =>
        new Dictionary<string, Func<string, List<RpcParams>?, Task<string>>>()
        {
            { nameof(RpcMethodKey.ping), PingAsync },
            { nameof(RpcMethodKey.rpcVersion), GetRpcVersionAsync },
            { nameof(RpcMethodKey.getCloudUsers), GetCloudAccountsAsync },
            { nameof(RpcMethodKey.getCloudRecordKey), GetReocrdTokenAsync },
        };

    public async Task<string> PingAsync(string key, List<RpcParams>? _param = null)
    {
        return "0";
    }

    public bool VerifyToken(List<RpcParams>? rpcParams = null)
    {
        MD5 md5 = MD5.Create();
        try
        {
            if (rpcParams == null)
                throw new ArgumentException("Verification failed");
            var token = rpcParams.FirstOrDefault(x => x.Key == "token")?.Value;
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Verification failed");
            if (AppSettings.RpcToken != Md5Helper.ComputeMd532(token))
            {
                throw new ArgumentException("Verification failed");
            }
            return true;
        }
        catch (Exception)
        {
            throw new ArgumentException("Verification failed");
        }
        finally
        {
            md5.Dispose();
        }
    }
}
