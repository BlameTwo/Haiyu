using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Waves.Api.Models;
using Waves.Api.Models.Rpc;

namespace Haiyu.ServiceHost;

public static class TaskExtensions
{
    public static async Task<T> WithCancellation<T>(
        this Task<T> task,
        CancellationToken cancellationToken
    )
    {
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var completedTask = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, linkedCts.Token));

        if (completedTask == task)
        {
            linkedCts.Cancel();
            return await task;
        }

        throw new OperationCanceledException(cancellationToken);
    }

    public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
    {
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var completedTask = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, linkedCts.Token));

        if (completedTask == task)
        {
            linkedCts.Cancel();
            await task;
        }
        else
        {
            throw new OperationCanceledException(cancellationToken);
        }
    }
}


public class RpcService : IHostedService
{
    private readonly ILogger<RpcService> _logger;
    private readonly SemaphoreSlim _connectionLimiter;
    private string _listenPrefix;
    private Task _listenLoopTask;

    private ArrayPool<byte> _arrayPool =  ArrayPool<byte>.Shared;

    public HttpListener SocketServer { get; private set; }

    public Dictionary<string, Func<string,List<RpcParams>, Task<string>>> Method { get; private set; }

    public RpcService(ILogger<RpcService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _listenPrefix = "http://localhost:9084/rpc/";
        int maxConnections = 10;
        _connectionLimiter = new SemaphoreSlim(maxConnections, maxConnections);
    }

    public void RegisterMethod(Dictionary<string, Func<string,List<RpcParams>, Task<string>>> Methods)
    {
        this.Method = Methods;
    }

    #region IHostedService 实现
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Rpc WebSocket service...");
        await InitRpcAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Rpc WebSocket service...");
        await CloseRpcAsync(cancellationToken);
    }
    #endregion

    #region IRpcService 实现
    public async Task InitRpcAsync(CancellationToken token = default)
    {
        if (SocketServer != null)
        {
            _logger.LogWarning("Rpc service is already running, restarting...");
            await CloseRpcAsync(token);
        }
        try
        {
            SocketServer = new HttpListener();
            SocketServer.Prefixes.Add(_listenPrefix);
            SocketServer.Start();
            _listenLoopTask = Task.Run(() => ListenForConnectionsAsync(token), token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Rpc WebSocket service");
            await CloseRpcAsync(token);
            throw;
        }
    }

    public async Task CloseRpcAsync(CancellationToken token = default)
    {
        try
        {
            if (SocketServer != null)
            {
                SocketServer.Stop();
                _logger.LogInformation("restart rpc core start");

                if (_listenLoopTask != null && !_listenLoopTask.IsCompleted)
                {
                    await _listenLoopTask.WithCancellation(token);
                }
                SocketServer.Close();
                SocketServer = null;
                _listenLoopTask = null;

                _logger.LogInformation("restart rpc core end");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while closing Rpc WebSocket service");
            throw;
        }
    }
    #endregion

    /// <summary>
    /// 获取是否支持远程连接
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> GetOpenConnectAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 设置远程连接
    /// </summary>
    /// <param name="value">设置值</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task SetOpenConnect(bool value)
    {
        throw new NotImplementedException();
    }
    private async Task ListenForConnectionsAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                HttpListenerContext context;
                try
                {
                    context = await SocketServer.GetContextAsync().WithCancellation(token);
                    if (!IsLocalClient(context.Request.RemoteEndPoint))
                    {
                        _logger.LogWarning(
                            "Rejected non-local connection attempt from {RemoteEndPoint}",
                            context.Request.RemoteEndPoint
                        );
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.Close();
                        continue;
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Listen loop canceled");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error waiting for client connections");
                    continue;
                }

                if (!context.Request.IsWebSocketRequest)
                {
                    _logger.LogWarning(
                        "Received non-WebSocket request from {RemoteEndPoint}",
                        context.Request.RemoteEndPoint
                    );
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.Close();
                    continue;
                }

                if (!await _connectionLimiter.WaitAsync(100, token))
                {
                    _logger.LogWarning(
                        "Connection rejected: Max connections reached. Client: {RemoteEndPoint}",
                        context.Request.RemoteEndPoint
                    );
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.Close();
                    continue;
                }

                WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(
                    subProtocol: null
                );
                WebSocket webSocket = webSocketContext.WebSocket;
                EndPoint remoteEndPoint = context.Request.RemoteEndPoint;

                _logger.LogInformation(
                    "New connection accepted: {RemoteEndPoint} (Active connections: {Count})",
                    remoteEndPoint,
                    _connectionLimiter.CurrentCount
                );

                _ = HandleSingleConnectionAsync(webSocket, remoteEndPoint, token);
            }
        }
        catch (Exception ex)
        {
            if (!token.IsCancellationRequested)
            {
                _logger.LogCritical(ex, "Critical error in listen loop");
            }
        }
    }

    private bool IsLocalClient(IPEndPoint remoteEndPoint)
    {
        //禁止远程访问
        if (remoteEndPoint is not IPEndPoint ipEndPoint)
        {
            _logger.LogWarning("Unsupported endpoint type: {EndpointType}", remoteEndPoint.GetType().Name);
            return false;
        }

        IPAddress clientIp = ipEndPoint.Address;

        bool isLocal = IPAddress.IsLoopback(clientIp);

        _logger.LogDebug(
            "Client IP: {ClientIp} (IsLocal: {IsLocal})",
            clientIp,
            isLocal
        );

        return isLocal;
    }

    private async Task HandleSingleConnectionAsync(
        WebSocket webSocket,
        EndPoint remoteEndPoint,
        CancellationToken token
    )
    {
        byte[]? buffer = null;
        try
        {
            while (webSocket.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                try
                {
                    buffer = _arrayPool.Rent(1024 * 1024);
                    result = await webSocket
                        .ReceiveAsync(new ArraySegment<byte>(buffer), token)
                        .WithCancellation(token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error receiving message from {RemoteEndPoint}",
                        remoteEndPoint
                    );
                    break;
                }
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogInformation(
                        "Client {RemoteEndPoint} requested close",
                        remoteEndPoint
                    );
                    await webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Client closed",
                        token
                    );
                    break;
                }
                
                string requestMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _logger.LogDebug(
                    "Received from {RemoteEndPoint}: {Message}",
                    remoteEndPoint,
                    requestMessage
                );
                string responseMessage = await ProcessRpcRequestAsync(
                    requestMessage,
                    remoteEndPoint,
                    token
                );
                if (!string.IsNullOrEmpty(responseMessage))
                {
                    byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(responseBuffer),
                        WebSocketMessageType.Text,
                        endOfMessage: true,
                        cancellationToken: token
                    );

                    _logger.LogDebug(
                        "Sent to {RemoteEndPoint}: {Message}",
                        remoteEndPoint,
                        responseMessage
                    );
                }
            }
        }
        finally
        {
            _arrayPool.Return(buffer);
            _connectionLimiter.Release();
            if (webSocket.State != WebSocketState.Closed)
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Service stopped",
                    CancellationToken.None
                );
            }

            _logger.LogInformation(
                "Connection closed: {RemoteEndPoint} (Active connections: {Count})",
                remoteEndPoint,
                _connectionLimiter.CurrentCount
            );
        }
    }

    private async Task<string> ProcessRpcRequestAsync(
        string requestMessage,
        EndPoint remoteEndPoint,
        CancellationToken token
    )
    {
        long jsonId = 0;
        try
        {
            var jsonObj = JsonSerializer.Deserialize<RpcRequest>(
                requestMessage,
                RpcContext.Default.RpcRequest
            );
            if (jsonObj == null)
                throw new ArgumentException();
            jsonId = jsonObj.RequestId;
            if (this.Method.TryGetValue(jsonObj.Method, out var task))
            {
                var result = await task.Invoke(jsonObj.Method,jsonObj.Params);
                return JsonSerializer.Serialize(
                    new RpcReponse() { RequestId = jsonId, Message = result,Success = true },
                    RpcContext.Default.RpcReponse
                );
            }
            return "";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Json Deserialize Error {RemoteEndPoint}", remoteEndPoint);
            return JsonSerializer.Serialize(
                new RpcReponse() { RequestId = jsonId, Message = ex.Message, Success = false },
                RpcContext.Default.RpcReponse
            );
        }
    }
}
