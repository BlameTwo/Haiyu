namespace WutheringWavesTool.ViewModel;

partial class SettingViewModel
{
    [RelayCommand]
    async Task SelectWallpaper()
    {
        await DialogManager.ShowWallpaperDialogAsync();
    }

}
