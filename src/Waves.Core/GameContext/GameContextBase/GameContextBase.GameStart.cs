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
        private System.Timers.Timer? gameRunTimer;
        private uint ppid;

        public async Task StartGameAsync()
        {
            try
            {
                string gameFolder = GameLocalConfig.GetConfig(
                    GameLocalSettingName.GameLauncherBassFolder
                );
                Process ps = new();
                ps.EnableRaisingEvents = true;
                _gameProcess?.Exited += Ps_Exited;
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
                gameRunTimer = new System.Timers.Timer();
                gameRunTimer.Interval = 1000;
                gameRunTimer.Elapsed += Time_Elapsed;
                gameRunTimer.Start();
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

        private async void Time_Elapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                ProcessScan.CheckGameAliveWithWin32(
                    Path.GetFileName(gameFile),
                    (uint)this.gameId,
                    out bool contained,
                    out uint ppid
                );
                if (contained)
                {
                    this.ppid = ppid;
                }
                else
                {
                    Ps_Exited(this, EventArgs.Empty);
                }
            }
            catch (Exception ex) { }
        }

        private async void Ps_Exited(object? sender, EventArgs e)
        {
            if (_gameProcess != null)
            {
                Logger.WriteInfo($"游戏退出代码{_gameProcess.ExitCode}");
                _gameProcess.Exited -= Ps_Exited;
                gameRunTimer?.Stop();
                gameRunTimer?.Dispose();
                this._isStarting = false;
                _gameProcess.Dispose();
                _gameProcess = null;
                _playGameTime = DateTime.MinValue;
            }
            if (gameContextOutputDelegate == null)
                return;
            await gameContextOutputDelegate
                .Invoke(this, new GameContextOutputArgs { Type = GameContextActionType.None })
                .ConfigureAwait(false);
        }

        public TimeSpan GetGameTime()
        {
            if (_playGameTime == DateTime.MinValue)
            {
                return TimeSpan.Zero;
            }
            return DateTime.Now - _playGameTime;
        }

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
