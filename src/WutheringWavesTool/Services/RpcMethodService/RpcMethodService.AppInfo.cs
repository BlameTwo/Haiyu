using System;
using System.Collections.Generic;
using System.Text;
using Waves.Api.Models.Rpc;

namespace Haiyu.Services;

public partial class RpcMethodService
{
    public Task<string> GetRpcVersionAsync(string key, List<RpcParams>? _param = null)
    {
        VerifyToken(_param);
        return Task.FromResult(AppSettings.RpcVersion);
    }

    
}
