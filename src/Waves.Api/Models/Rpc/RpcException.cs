namespace Waves.Api.Models.Rpc;

/// <summary>
/// RPC执行错误响应
/// </summary>
public class RpcException : Exception
{
    public int Code { get; set; }

    public RpcException(int code, bool success, string message)
        : base(message)
    {
        Code = code;
        Success = success;
    }

    public bool Success { get; set; }
}
