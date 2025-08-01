namespace WutheringWavesTool.ViewModel.GameViewModels;

partial class GameContextViewModelBase
{
    [RelayCommand]
    async Task UpdateGameAsync()
    {
        if (_bthType == 3)
        {
            await GameContext.StartGameAsync();
        }
        if (_bthType == 4)
        {
            var localVersion = await GameContext.GameLocalConfig.GetConfigAsync(
                GameLocalSettingName.LocalGameVersion
            );
            var result = Version.Parse(this.DisplayVersion) > Version.Parse(localVersion);
            await GameContext.UpdateGameAsync();
        }
        if (_bthType == 5)
        {
            await GameContext.StopGameAsync();
        }
    }
}
