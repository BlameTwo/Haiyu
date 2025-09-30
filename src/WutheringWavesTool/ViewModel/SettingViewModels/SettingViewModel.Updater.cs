namespace Haiyu.ViewModel;

partial class SettingViewModel
{
    [RelayCommand]
    async Task UpdateVersion()
    {
        var newVersion = await VersionUpdater.GetLasterPackageAsync(this.CTS.Token);
        if(newVersion == null)
        {
            TipShow.ShowMessage("无法获取版本信息",Symbol.Clear);
            return;
        }
        TipShow.ShowMessage("最新版本为"+newVersion.TagName, Symbol.Page);
    }
}
