using Waves.Api.Models.Rpc;

namespace Haiyu.Services.Contracts;

/// <summary>
/// RPC 方法服务
/// </summary>
public interface IRpcMethodService
{
    public Dictionary<string, Func<string,List<RpcParams>,Task<string>>> Method { get; }

}
