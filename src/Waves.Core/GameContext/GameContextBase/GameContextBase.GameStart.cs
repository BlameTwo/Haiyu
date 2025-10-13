using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Waves.Core.Models;
using Waves.Core.Models.Enums;

namespace Waves.Core.GameContext
{
    public partial class GameContextBase
    {
        Process? _gameProcess = null;
        private bool _isStarting;
        private DateTime _playGameTime = DateTime.MinValue;

        public async Task StartGameAsync()
        {
            try
            {
                string gameProgram = GameLocalConfig.GetConfig(
                    GameLocalSettingName.GameLauncherBassProgram
                );
                string gameFolder = GameLocalConfig.GetConfig(
                    GameLocalSettingName.GameLauncherBassFolder
                );
                Process ps = new();
                ps.EnableRaisingEvents = true;
                ProcessStartInfo info = new(gameFolder + "\\" + this.Config.GameExeName)
                {
                    //Arguments = "Client -dx12",
                    Arguments = "Client -dx12",
                    WorkingDirectory = gameFolder,
                    UseShellExecute = true,
                    Verb = "runas",
                };

                this._gameProcess = ps;
                _gameProcess.Exited += Ps_Exited;
                _gameProcess.StartInfo = info;
                _gameProcess.Start();
                this._isStarting = true;
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

        private async void Ps_Exited(object? sender, EventArgs e)
        {
            if (_gameProcess != null)
            {
                Logger.WriteInfo($"游戏退出代码{_gameProcess.ExitCode}");
                _gameProcess.Exited -= Ps_Exited;
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
            _gameProcess?.Kill(true);
            if (_gameProcess != null)
                await _gameProcess.WaitForExitAsync();
            Logger.WriteInfo("退出游戏………………");
        }
    }
}
