using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace Haiyu.ViewModel.GameViewModels;

partial class KuroGameContextViewModel
{
    [RelayCommand]
    async Task UpdateGameAsync()
    {
        if (_bthType == 3)
        {
            if( await GameContext.StartGameAsync())
            {
                this.AppContext.MinToTaskbar();
                
            }
        }
        if (_bthType == 4)
        {
            var localVersion = await GameContext.GameLocalConfig.GetConfigAsync(
                GameLocalSettingName.LocalGameVersion
            );
            var result = Version.Parse(this.DisplayVersion) > Version.Parse(localVersion);
            await GameContext.UpdataGameAsync();
        }
        if (_bthType == 5)
        {
            //await GameContext.StopGameAsync();
        }
    }
}
