namespace WutheringWavesTool.ViewModel;

partial class SettingViewModel
{
    [RelayCommand]
    async Task SelectWallpaper()
    {
        await DialogManager.ShowWallpaperDialogAsync();
    }

    [ObservableProperty]
    public partial ObservableCollection<string> Themes { get; set; } = ["Default", "Light", "Dark"];

    [ObservableProperty]
    public partial string SelectTheme { get; set; }

    
}
