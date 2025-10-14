using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CommunityToolkit.Mvvm.Input;
using Waves.Core.Common;
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext
{
    public partial class GameContextBase
    {
        Process? _gameProcess = null;
        private bool _isStarting;
        private int gameId;
        private string gameFile;
        private DateTime _playGameTime = DateTime.MinValue;
        private System.Threading.Timer? gameRunTimer;
        private uint ppid;

        public async Task StartGameAsync()
        {
            try
            {
                string gameFolder = GameLocalConfig.GetConfig(
                    GameLocalSettingName.GameLauncherBassFolder
                );
                Process ps = new();
                ProcessStartInfo info = new(gameFolder + "\\" + this.Config.GameExeName)
                {
                    Arguments = "Client -dx12",
                    WorkingDirectory = gameFolder,
                    Verb = "runas",
                    UseShellExecute = true,
                };
                this._gameProcess = ps;
                _gameProcess.StartInfo = info;
                _gameProcess.Start();
                this._isStarting = true;
                this.gameId = _gameProcess.Id;
                this.gameFile = info.FileName;
                gameRunTimer = new System.Threading.Timer(
                    callback: async _ => await CheckGameStatusAsync(),
                    state: null,
                    dueTime: 3000,
                    period: 3000
                );
                Logger.WriteInfo("正在启动游戏……");
            }
            catch (Exception ex)
            {
                this._isStarting = false;
                Logger.WriteError($"游戏启动错误{ex.Message}");
            }
            if (gameContextOutputDelegate == null)
                return;
            await gameContextOutputDelegate
                .Invoke(this, new GameContextOutputArgs { Type = GameContextActionType.None })
                .ConfigureAwait(false);
        }

        private async Task CheckGameStatusAsync()
        {
            try
            {

                var result = await Task.Run(() =>
                {
                    ProcessScan.CheckGameAliveWithWin32(
                        Path.GetFileName(gameFile),
                        (uint)this.gameId,
                        out bool contained,
                        out uint ppid,
                        out var filepath
                    );
                    return (contained, ppid);
                });

                if (!result.contained)
                {
                    await OnGameExited();
                }
                else
                {
                    this.ppid = result.ppid;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"检查游戏状态失败: {ex.Message}");
            }
        }

        private async Task OnGameExited()
        {
            // 清理资源
            gameRunTimer?.Dispose();
            _gameProcess?.Dispose();
            _gameProcess = null;
            _isStarting = false;
            Logger.WriteInfo($"游戏已退出，游戏运行时长:{GetGameTime():G}");
            // 异步通知
            if (gameContextOutputDelegate != null)
            {
                await gameContextOutputDelegate.Invoke(this, new GameContextOutputArgs { Type = GameContextActionType.None })
                    .ConfigureAwait(false);
            }
        }

        public TimeSpan GetGameTime()
        {
            if (_playGameTime == DateTime.MinValue)
            {
                return TimeSpan.Zero;
            }
            return DateTime.Now - _playGameTime;
        }

        [Obsolete("该方法已过时，由于非管理员权限设置，无法进行关闭进程")]
        public async Task StopGameAsync()
        {
            if (!this._isStarting && _gameProcess == null)
            {
                return;
            }
            Process.GetProcessById((int)ppid).Kill();
            _gameProcess?.Kill(true);
            Logger.WriteInfo("退出游戏………………");
            this._isStarting = false;
            await gameContextOutputDelegate
                .Invoke(this, new GameContextOutputArgs { Type = GameContextActionType.None })
                .ConfigureAwait(false);
        }
    }
}
