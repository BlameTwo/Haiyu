using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Core;

namespace WutheringWavesTool.ViewModel;

partial class SettingViewModel
{
    [RelayCommand]
    async Task SelectWallpaper()
    {
        await DialogManager.ShowWallpaperDialogAsync();
    }

    public string OldCursorName { get; set; }

    [ObservableProperty]
    public partial string SelectCursorName { get; set; }

    public List<string> Cursors { get; set; } = ["默认","弗糯糯","卡提西亚","守岸人"];

    async partial void OnSelectCursorNameChanging(string value)
    {
        if (value == OldCursorName)
            return;
        if((await DialogManager.ShowMessageDialog("该选项需要重启","重启","取消")) == ContentDialogResult.Primary)
        {
            AppSettings.SelectCursor = value;
            AppRestartFailureReason restartError = AppInstance.Restart(null);
        }
        else
        {
            SelectCursorName = Cursors.Find(x => x == OldCursorName);
        }
    }
}
