using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.Graphics.Display;

namespace Haiyu.Services
{
    public class XboxGameController : Contracts.IGameController, IHostedService
    {
        // ==================== Win32 API 定义 ====================
        // 获取系统度量（屏幕分辨率）
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        // 模拟鼠标输入（核心）
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // 获取当前鼠标位置（有效API）
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        // 系统度量常量（屏幕宽度/高度）
        private const int SM_CXSCREEN = 0;  // 主屏幕宽度（像素）
        private const int SM_CYSCREEN = 1;  // 主屏幕高度（像素）

        // 鼠标输入结构体定义
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint Type;
            public MOUSEINPUT Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int X; // 绝对坐标时：0~65535；相对坐标时：像素增量
            public int Y; // 同上
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        // 鼠标控制常量
        private const uint INPUT_MOUSE = 0;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;
        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int WHEEL_DELTA = 120;

        // ==================== 类成员变量 ====================
        // 屏幕分辨率（通过有效API获取）
        private int _screenWidth;
        private int _screenHeight;

        public Gamepad? GamePad { get; private set; }
        public bool IsInit { get; private set; }
        private CancellationTokenSource? _cts;
        private Task? _monitoringTask;
        private GamepadReading _lastReading;

        // 摇杆死区/防抖
        private const double ThumbstickDeadzone = 0.2f;
        private const int ThumbstickDebounceTime = 100;

        // 左/右摇杆日志状态
        private DateTime _lastLeftThumbstickLogTime = DateTime.MinValue;
        private bool _isLeftThumbstickCentered = true;
        private DateTime _lastRightThumbstickLogTime = DateTime.MinValue;
        private bool _isRightThumbstickCentered = true;

        // 鼠标控制参数
        private const double MouseMoveSpeed = 6.0f;
        private const double MouseScrollSpeed = 1.5f;
        private const double MouseSmoothingFactor = 0.9f;
        private double _lastMouseDeltaX = 0f;
        private double _lastMouseDeltaY = 0f;

        // ==================== 构造函数 ====================
        public XboxGameController()
        {
            // 获取屏幕分辨率（仅用Win32 API，不依赖窗口焦点）
            _screenWidth = GetSystemMetrics(SM_CXSCREEN);
            _screenHeight = GetSystemMetrics(SM_CYSCREEN);

            // 兜底：防止获取失败
            if (_screenWidth == 0 || _screenHeight == 0)
            {
                _screenWidth = 1920;
                _screenHeight = 1080;
            }

            Debug.WriteLine($"[屏幕信息] 分辨率：{_screenWidth}x{_screenHeight}");
        }

        // ==================== 辅助方法 ====================
        #region 辅助方法：屏幕像素坐标转SendInput绝对坐标（0~65535）
        private int PixelToAbsoluteX(int pixelX)
        {
            return (int)((pixelX / (double)_screenWidth) * 65535);
        }

        private int PixelToAbsoluteY(int pixelY)
        {
            return (int)((pixelY / (double)_screenHeight) * 65535);
        }
        #endregion

        #region 辅助方法：归一化摇杆值
        private double NormalizeThumbstickValue(double rawValue, double magnitude)
        {
            if (magnitude < ThumbstickDeadzone) return 0;
            return (rawValue / magnitude) * (1.0f / (1.0f - ThumbstickDeadzone));
        }
        #endregion

        #region 新增：全局枚举Xbox手柄（脱离窗口焦点）
        private Gamepad? GetGlobalXboxGamepad()
        {
            try
            {
                // 枚举系统中所有已连接的手柄（全局生效，不依赖窗口焦点）
                var allGamepads = Gamepad.Gamepads;
                if (allGamepads.Count > 0)
                {
                    // 优先选第一个Xbox手柄（兼容不同名称的Xbox手柄）
                    return allGamepads.FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        // ==================== 核心逻辑 ====================
        #region 核心：全局轮询手柄（绕过焦点限制）
        private void StartMonitoringGamepad()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            // 提升线程优先级，确保后台稳定运行
            _monitoringTask = Task.Factory.StartNew(async () =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        // 关键：每次循环主动获取全局手柄（不依赖初始绑定）
                        GamePad = GetGlobalXboxGamepad();
                        if (GamePad == null)
                        {
                            await Task.Delay(100, token); // 无手柄时降低轮询频率
                            continue;
                        }

                        // 读取手柄状态（即使窗口失焦也能获取）
                        var currentReading = GamePad.GetCurrentReading();

                        // 处理按键和摇杆输入
                        DetectButtonPresses(currentReading);
                        DetectThumbstickMovementWithMouseControl(currentReading);

                        _lastReading = currentReading;
                        await Task.Delay(16, token); // 60帧/秒，保证响应丝滑
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.WriteLine("[手柄日志] 手柄监控任务已取消");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[手柄日志] 监控异常: {ex.Message}");
                        await Task.Delay(16, token); // 异常后继续循环，不终止
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        #endregion

        #region 处理手柄按键（A键模拟鼠标左键）
        private void DetectButtonPresses(GamepadReading currentReading)
        {
            var pressedButtons = currentReading.Buttons & ~_lastReading.Buttons;

            // A键模拟鼠标左键单击
            if (pressedButtons == GamepadButtons.A)
            {
                // 模拟左键按下
                INPUT leftDownInput = new INPUT
                {
                    Type = INPUT_MOUSE,
                    Data = new MOUSEINPUT
                    {
                        X = 0,
                        Y = 0,
                        MouseData = 0,
                        Flags = 0x0002, // MOUSEEVENTF_LEFTDOWN
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                };

                // 模拟左键释放
                INPUT leftUpInput = new INPUT
                {
                    Type = INPUT_MOUSE,
                    Data = new MOUSEINPUT
                    {
                        X = 0,
                        Y = 0,
                        MouseData = 0,
                        Flags = 0x0004, // MOUSEEVENTF_LEFTUP
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                };

                // 发送点击事件
                SendInput(1, new[] { leftDownInput }, Marshal.SizeOf(typeof(INPUT)));
                Task.Delay(10).Wait(); // 模拟真实点击间隔
                SendInput(1, new[] { leftUpInput }, Marshal.SizeOf(typeof(INPUT)));

                Debug.WriteLine("[手柄日志] 按下A键，模拟鼠标左键单击");
            }

            // 输出按键日志
            if (pressedButtons != GamepadButtons.None)
            {
                Debug.WriteLine($"[手柄日志] 按下按键: {pressedButtons}");
            }
        }
        #endregion

        #region 处理摇杆输入（左摇杆移鼠标，右摇杆滚滚轮）
        private void DetectThumbstickMovementWithMouseControl(GamepadReading currentReading)
        {
            #region 1. 左摇杆控制鼠标移动（修复hover问题）
            var leftMagnitude = (double)Math.Sqrt(
                Math.Pow(currentReading.LeftThumbstickX, 2) +
                Math.Pow(currentReading.LeftThumbstickY, 2)
            );
            bool isLeftCenteredNow = leftMagnitude < ThumbstickDeadzone;

            if (!isLeftCenteredNow)
            {
                // 归一化摇杆值
                double normalizedX = NormalizeThumbstickValue(currentReading.LeftThumbstickX, leftMagnitude);
                double normalizedY = NormalizeThumbstickValue(currentReading.LeftThumbstickY, leftMagnitude);

                // 获取当前鼠标位置
                if (GetCursorPos(out POINT currentMousePos))
                {
                    // 平滑增量计算
                    double targetDeltaX = normalizedX * MouseMoveSpeed * leftMagnitude;
                    double targetDeltaY = -normalizedY * MouseMoveSpeed * leftMagnitude;
                    _lastMouseDeltaX = targetDeltaX * MouseSmoothingFactor + _lastMouseDeltaX * (1 - MouseSmoothingFactor);
                    _lastMouseDeltaY = targetDeltaY * MouseSmoothingFactor + _lastMouseDeltaY * (1 - MouseSmoothingFactor);

                    // 计算新鼠标位置并限制在屏幕内
                    int newPixelX = currentMousePos.X + (int)Math.Round(_lastMouseDeltaX);
                    int newPixelY = currentMousePos.Y + (int)Math.Round(_lastMouseDeltaY);
                    newPixelX = Math.Clamp(newPixelX, 0, _screenWidth - 1);
                    newPixelY = Math.Clamp(newPixelY, 0, _screenHeight - 1);

                    // 发送鼠标移动事件（触发hover动画）
                    INPUT mouseInput = new INPUT
                    {
                        Type = INPUT_MOUSE,
                        Data = new MOUSEINPUT
                        {
                            X = PixelToAbsoluteX(newPixelX),
                            Y = PixelToAbsoluteY(newPixelY),
                            MouseData = 0,
                            Flags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
                        }
                    };
                    SendInput(1, new[] { mouseInput }, Marshal.SizeOf(typeof(INPUT)));
                }
            }
            else
            {
                // 摇杆回中，重置平滑增量
                _lastMouseDeltaX = 0f;
                _lastMouseDeltaY = 0f;
            }
            #endregion

            #region 2. 右摇杆控制鼠标滚轮（修复边界BUG）
            double rightY = currentReading.RightThumbstickY;
            double rightYMagnitude = Math.Abs(rightY);
            bool isRightCenteredNow = rightYMagnitude < ThumbstickDeadzone;

            if (!isRightCenteredNow)
            {
                double normalizedRightY = NormalizeThumbstickValue(rightY, rightYMagnitude);
                int scrollDelta = (int)(normalizedRightY * MouseScrollSpeed * WHEEL_DELTA / 10);
                if (scrollDelta != 0)
                {
                    INPUT scrollInput = new INPUT
                    {
                        Type = INPUT_MOUSE,
                        Data = new MOUSEINPUT
                        {
                            X = 0,
                            Y = 0,
                            MouseData = (uint)scrollDelta,
                            Flags = MOUSEEVENTF_WHEEL,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
                        }
                    };
                    SendInput(1, new[] { scrollInput }, Marshal.SizeOf(typeof(INPUT)));
                }
            }
            #endregion

            #region 3. 左/右摇杆日志输出
            // 左摇杆日志
            bool canLogLeft = (DateTime.Now - _lastLeftThumbstickLogTime).TotalMilliseconds > ThumbstickDebounceTime;
            if (canLogLeft)
            {
                if (_isLeftThumbstickCentered && !isLeftCenteredNow)
                {
                    Debug.WriteLine($"[手柄日志] 左摇杆移出中心 - X: {currentReading.LeftThumbstickX:F2}, Y: {currentReading.LeftThumbstickY:F2}, 幅度: {leftMagnitude:F2}");
                    _lastLeftThumbstickLogTime = DateTime.Now;
                    _isLeftThumbstickCentered = false;
                }
                else if (!_isLeftThumbstickCentered && isLeftCenteredNow)
                {
                    Debug.WriteLine($"[手柄日志] 左摇杆回到中心");
                    _lastLeftThumbstickLogTime = DateTime.Now;
                    _isLeftThumbstickCentered = true;
                }
                else if (!isLeftCenteredNow)
                {
                    var lastLeftMagnitude = (double)Math.Sqrt(
                        Math.Pow(_lastReading.LeftThumbstickX, 2) +
                        Math.Pow(_lastReading.LeftThumbstickY, 2)
                    );
                    var magnitudeDelta = Math.Abs(leftMagnitude - lastLeftMagnitude);

                    if (magnitudeDelta > 0.15)
                    {
                        Debug.WriteLine($"[手柄日志] 左摇杆移动 - X: {currentReading.LeftThumbstickX:F2}, Y: {currentReading.LeftThumbstickY:F2}, 幅度: {leftMagnitude:F2}");
                        _lastLeftThumbstickLogTime = DateTime.Now;
                    }
                }
            }

            // 右摇杆日志
            var rightMagnitude = (double)Math.Sqrt(
                Math.Pow(currentReading.RightThumbstickX, 2) +
                Math.Pow(currentReading.RightThumbstickY, 2)
            );
            bool canLogRight = (DateTime.Now - _lastRightThumbstickLogTime).TotalMilliseconds > ThumbstickDebounceTime;
            if (canLogRight)
            {
                if (_isRightThumbstickCentered && !isRightCenteredNow)
                {
                    Debug.WriteLine($"[手柄日志] 右摇杆移出中心 - Y: {currentReading.RightThumbstickY:F2}, 幅度: {rightMagnitude:F2}");
                    _lastRightThumbstickLogTime = DateTime.Now;
                    _isRightThumbstickCentered = false;
                }
                else if (!_isRightThumbstickCentered && isRightCenteredNow)
                {
                    Debug.WriteLine($"[手柄日志] 右摇杆回到中心");
                    _lastRightThumbstickLogTime = DateTime.Now;
                    _isRightThumbstickCentered = true;
                }
                else if (!isRightCenteredNow)
                {
                    var lastRightMagnitude = (double)Math.Sqrt(
                        Math.Pow(_lastReading.RightThumbstickX, 2) +
                        Math.Pow(_lastReading.RightThumbstickY, 2)
                    );
                    var magnitudeDelta = Math.Abs(rightMagnitude - lastRightMagnitude);

                    if (magnitudeDelta > 0.15)
                    {
                        Debug.WriteLine($"[手柄日志] 右摇杆移动 - Y: {currentReading.RightThumbstickY:F2}, 幅度: {rightMagnitude:F2}");
                        _lastRightThumbstickLogTime = DateTime.Now;
                    }
                }
            }
            #endregion
        }
        #endregion

        // ==================== IHostedService 实现 ====================
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsInit)
            {
                _cts?.Cancel();
                IsInit = false;
            }

            // 直接启动全局手柄轮询（无需事件绑定）
            StartMonitoringGamepad();

            IsInit = true;
            Debug.WriteLine("[手柄日志] 全局手柄监听已启动（后台也生效）");
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts?.Cancel();
            if (_monitoringTask != null)
            {
                await _monitoringTask;
            }

            GamePad = null;
            _cts?.Dispose();
            _cts = null;
            IsInit = false;

            Debug.WriteLine("[手柄日志] 手柄服务已停止");
            await Task.CompletedTask;
        }
    }
}