using Haiyu.Common.WindowContext;

namespace Haiyu.Common.Contracts;

public sealed class WindowSession : IDisposable
{
    private readonly TaskCompletionSource<object?> _completionSource = new(
        TaskCreationOptions.RunContinuationsAsynchronously
    );
    private readonly object _syncRoot = new();

    private Window? _window;
    private WindowContext.WindowContext? _winContext;

    public WindowContext.WindowContext Context =>
        _winContext ?? throw new InvalidOperationException("WindowSession 尚未绑定窗口上下文。");

    private object? _result;
    private bool _isClosed;
    private bool _isDisposed;

    public object? Result
    {
        get
        {
            lock (_syncRoot)
            {
                return _result;
            }
        }
    }

    public bool IsClosed
    {
        get
        {
            lock (_syncRoot)
            {
                return _isClosed;
            }
        }
    }

    public Task<object?> Completion => _completionSource.Task;

    internal void Attach(Window window, WindowContext.WindowContext winContext)
    {
        ArgumentNullException.ThrowIfNull(window);
        ArgumentNullException.ThrowIfNull(winContext);

        lock (_syncRoot)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            if (_window is not null)
            {
                throw new InvalidOperationException("当前 WindowSession 已绑定 Window。");
            }

            if (_isClosed)
            {
                throw new InvalidOperationException("已结束的 WindowSession 不能再次绑定 Window。");
            }

            _window = window;
            _winContext = winContext;
            _window.Closed += Window_Closed;
        }
    }

    public void Close(object? result = null)
    {
        Window? window;

        lock (_syncRoot)
        {
            if (_isClosed || _isDisposed)
            {
                return;
            }

            _isClosed = true;
            _result = result;
            window = _window;
        }

        _completionSource.TrySetResult(result);
        window?.Close();
    }

    public TParameter GetParameter<TParameter>()
    {
        if (Context.Parameter is TParameter parameter)
        {
            return parameter;
        }

        throw new InvalidOperationException(
            $"窗口参数不是 {typeof(TParameter).FullName} 类型。");
    }

    public async Task<TResult?> GetResultAsync<TResult>()
    {
        var result = await Completion.ConfigureAwait(false);
        return result is TResult typedResult ? typedResult : default;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        Complete(null);
    }

    private void Complete(object? result)
    {
        lock (_syncRoot)
        {
            if (_isClosed || _isDisposed)
            {
                return;
            }

            _isClosed = true;
            _result = result;
        }

        _completionSource.TrySetResult(result);
    }

    internal void Detach()
    {
        Window? window;

        lock (_syncRoot)
        {
            window = _window;
            _window = null;
            _winContext = null;
        }

        if (window is not null)
        {
            window.Closed -= Window_Closed;
        }
    }

    public void Dispose()
    {
        object? result;

        lock (_syncRoot)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _isClosed = true;
            result = _result;
        }

        Detach();
        _completionSource.TrySetResult(result);
    }
}
