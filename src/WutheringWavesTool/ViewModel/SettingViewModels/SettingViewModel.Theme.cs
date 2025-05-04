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

    [RelayCommand]
    void SetTheme()
    {
        switch (SelectTheme)
        {
            case "Default":
                this.AppContext.SetElementTheme(ElementTheme.Default);
                break;
            case "Light":
                this.AppContext.SetElementTheme(ElementTheme.Light);
                break;
            case "Dark":
                this.AppContext.SetElementTheme(ElementTheme.Dark);
                break;
        }
        AppSettings.AppTheme = SelectTheme;
    }
}
