using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace Haiyu.ViewModel.GameViewModels;

partial class GameContextViewModelBase
{
    [RelayCommand]
    async Task UpdateGameAsync()
    {
        if (_bthType == 3)
        {
            if( await GameContext.StartGameAsync())
            {
                this.AppContext.MinToTaskbar();
                AppNotification notify = new AppNotificationBuilder()
                    .AddText($"游戏已经启动，程序已最小化到任务栏")
                    .BuildNotification();
                AppNotificationManager.Default.Show(notify);
            }
        }
        if (_bthType == 4)
        {
            var localVersion = GameContext.GameLocalConfig.GetConfig(
                GameLocalSettingName.LocalGameVersion
            );
            var result = Version.Parse(this.DisplayVersion) > Version.Parse(localVersion);
            await GameContext.UpdateGameAsync();
        }
        if (_bthType == 5)
        {
            //await GameContext.StopGameAsync();
        }
    }
}
