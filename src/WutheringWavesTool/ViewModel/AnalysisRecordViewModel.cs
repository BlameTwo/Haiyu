using Waves.Api.Models.CloudGame;

namespace WutheringWavesTool.ViewModel;

public partial class AnalysisRecordViewModel : ViewModelBase
{
    public AnalysisRecordViewModel(ICloudGameService cloudGameService)
    {
        CloudGameService = cloudGameService;
    }

    public bool Loading { get; private set; }
    public ICloudGameService CloudGameService { get; }
    public LoginData LoginData { get; internal set; }

    [RelayCommand]
    async Task Loaded()
    {
        await RefreshAsync();
    }

    [RelayCommand]
    async Task RefreshAsync()
    {
        this.Loading = true;
        var result = await CloudGameService.OpenUserAsync(this.LoginData);
        if (!result.Item1)
            return;
        var recordId = (await CloudGameService.GetRecordAsync(this.CTS.Token));
        var roleActivity = (
            await CloudGameService.GetGameRecordResource(
                recordId.Data.RecordId,
                recordId.Data.PlayerId.ToString(),
                1
            )
        )
            .Data.Select(x => new RecordCardItemWrapper(x))
            .ToObservableCollection();
        var weaponActiviy = (
            await CloudGameService.GetGameRecordResource(
                recordId.Data.RecordId,
                recordId.Data.PlayerId.ToString(),
                2
            )
        )
            .Data.Select(x => new RecordCardItemWrapper(x))
            .ToObservableCollection();
        var roleDaily = (
            await CloudGameService.GetGameRecordResource(
                recordId.Data.RecordId,
                recordId.Data.PlayerId.ToString(),
                3
            )
        )
            .Data.Select(x => new RecordCardItemWrapper(x))
            .ToObservableCollection();
        var weaponDaily = (
            await CloudGameService.GetGameRecordResource(
                recordId.Data.RecordId,
                recordId.Data.PlayerId.ToString(),
                4
            )
        )
            .Data.Select(x => new RecordCardItemWrapper(x))
            .ToObservableCollection();

        var FiveGroup = await RecordHelper.GetFiveGroupAsync();
        var AllRole = await RecordHelper.GetAllRoleAsync();
        var AllWeapon = await RecordHelper.GetAllWeaponAsync();
        var StartRole = RecordHelper.FormatFiveRoleStar(FiveGroup);
        var StartWeapons = RecordHelper.FormatFiveWeaponeRoleStar(FiveGroup);
        #region 计算下一次保底

        #endregion
        this.Loading = false;
    }
}
