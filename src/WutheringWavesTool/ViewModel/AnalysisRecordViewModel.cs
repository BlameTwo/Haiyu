using System.Globalization;
using System.Linq;
using Haiyu.Contracts;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using Waves.Api.Models.CloudGame;

namespace Haiyu.ViewModel;

public partial class AnalysisRecordViewModel : ViewModelBase
{
    private readonly List<RecordCardItemWrapper> roleActivity = new();
    private readonly List<RecordCardItemWrapper> weaponActiviy = new();
    private readonly List<RecordCardItemWrapper> roleDaily = new();
    private readonly List<RecordCardItemWrapper> weaponDaily = new();

    public AnalysisRecordViewModel(ICloudGameService cloudGameService)
    {
        CloudGameService = cloudGameService;
    }

    [ObservableProperty]
    public partial Visibility LoadingVisibility { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<DateTimePoint> AllPoints { get; set; } = new();

    [ObservableProperty]
    public partial Visibility DataVisibility { get; set; }
    public ICloudGameService CloudGameService { get; }
    public LoginData LoginData { get; internal set; }

    [RelayCommand]
    async Task Loaded()
    {
        await RefreshAsync();
    }

    [ObservableProperty]
    public partial long RoleActivityAllCount { get; set; } = 0;

    [ObservableProperty]
    public partial long WeaponActivityAllCount { get; set; } = 0;

    [ObservableProperty]
    public partial long DailyAllCount { get; set; }

    [ObservableProperty]
    public partial long RoleActivityCount { get; set; } = 0;

    [ObservableProperty]
    public partial long WeaponActivityCount { get; set; } = 0;

    [ObservableProperty]
    public partial long RoleActivityCount2 { get; set; } = 0;

    [ObservableProperty]
    public partial double CurrentRoleDeily { get; set; } = 0;

    [ObservableProperty]
    public partial double CurrentWeaponDeily { get; set; } = 0;

    [ObservableProperty]
    public partial long ExpectedRoleDeily { get; set; } = 0;

    [ObservableProperty]
    public partial long ExpectedActivityWeaponDeily { get; set; } = 0;

    [ObservableProperty]
    public partial long ExpectedActivityRoleDeily { get; set; } = 0;

    [ObservableProperty]
    public partial long ExpectedDailyWeaponDeily { get; set; } = 0;

    [ObservableProperty]
    public partial ObservableCollection<GameRecordNavigationItem> RecordNavigationItems { get; set; } =
        GameRecordNavigationItem.FourDefault;

    [ObservableProperty]
    public partial GameRecordNavigationItem SelectNavigationItem { get; set; }

    [ObservableProperty]
    public partial double Guaranteed { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<RecordActivityFiveStarItemWrapper> StarItems { get; set; }
    public FiveGroupModel FiveGroup { get; private set; }
    public List<CommunityRoleData> AllRole { get; private set; }
    public List<CommunityWeaponData> AllWeapon { get; private set; }
    public List<int> StartRole { get; private set; }
    public List<int> StartWeapons { get; private set; }

    /// <summary>
    /// 星级分布
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<object> StarPipeDatas { get; set; } = new();

    /// <summary>
    /// 大保底与小保底占比
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<object> RangePipeDatas { get; set; } = new();

    partial void OnSelectNavigationItemChanged(GameRecordNavigationItem value)
    {
        if (value == null)
            return;
        AllPoints.Clear();
        switch (value.Id)
        {
            case 1:
                StarItems = RecordHelper
                    .FormatStartFive(
                        this.roleActivity,
                        RecordHelper.FormatFiveRoleStar(this.FiveGroup)
                    )
                    .Item1.Format(this.AllRole)
                    .ToCardItemObservableCollection();
                break;
            case 2:
                StarItems = RecordHelper
                    .FormatStartFive(
                        this.weaponActiviy,
                        RecordHelper.FormatFiveWeaponeRoleStar(this.FiveGroup)
                    )
                    .Item1.Format(this.AllWeapon)
                    .ToCardItemObservableCollection();
                break;
            case 3:
                StarItems = RecordHelper
                    .FormatStartFive(
                        this.roleDaily,
                        RecordHelper.FormatFiveRoleStar(this.FiveGroup)
                    )
                    .Item1.Format(this.AllRole)
                    .ToCardItemObservableCollection();
                break;
            case 4:
                StarItems = RecordHelper
                    .FormatStartFive(
                        this.weaponDaily,
                        RecordHelper.FormatFiveRoleStar(this.FiveGroup)
                    )
                    .Item1.Format(this.AllWeapon)
                    .ToCardItemObservableCollection();
                break;
        }
    }

    [RelayCommand]
    async Task RefreshAsync()
    {
        LoadingVisibility = Visibility.Visible;
        DataVisibility = Visibility.Collapsed;
        #region 刷新数据源
        roleActivity.Clear();
        weaponActiviy.Clear();
        roleDaily.Clear();
        weaponDaily.Clear();
        RoleActivityAllCount = 0;
        RoleActivityCount = 0;
        RoleActivityCount2 = 0;
        WeaponActivityAllCount = 0;
        WeaponActivityCount = 0;
        var result = await CloudGameService.OpenUserAsync(this.LoginData);
        if (!result.Item1)
            return;
        var recordId = (await CloudGameService.GetRecordAsync(this.CTS.Token));
        roleActivity.AddRange(
            (
                await CloudGameService.GetGameRecordResource(
                    recordId.Data.RecordId,
                    recordId.Data.PlayerId.ToString(),
                    1
                )
            ).Data.Select(x => new RecordCardItemWrapper(x))
        );
        weaponActiviy.AddRange(
            (
                await CloudGameService.GetGameRecordResource(
                    recordId.Data.RecordId,
                    recordId.Data.PlayerId.ToString(),
                    2
                )
            ).Data.Select(x => new RecordCardItemWrapper(x))
        );
        roleDaily.AddRange(
            (
                await CloudGameService.GetGameRecordResource(
                    recordId.Data.RecordId,
                    recordId.Data.PlayerId.ToString(),
                    3
                )
            ).Data.Select(x => new RecordCardItemWrapper(x))
        );
        weaponDaily.AddRange(
            (
                await CloudGameService.GetGameRecordResource(
                    recordId.Data.RecordId,
                    recordId.Data.PlayerId.ToString(),
                    4
                )
            ).Data.Select(x => new RecordCardItemWrapper(x))
        );
        #endregion
        FiveGroup = await RecordHelper.GetFiveGroupAsync();
        AllRole = await RecordHelper.GetAllRoleAsync();
        AllWeapon = await RecordHelper.GetAllWeaponAsync();
        StartRole = RecordHelper.FormatFiveRoleStar(FiveGroup);
        StartWeapons = RecordHelper.FormatFiveWeaponeRoleStar(FiveGroup);
        #region 计算
        RoleActivityAllCount = roleActivity.Count;
        WeaponActivityAllCount = weaponActiviy.Count;
        DailyAllCount = roleDaily.Count + weaponDaily.Count;
        var ruleActiv = RecordHelper.FormatRecordFive(this.roleActivity);
        var weaponActiv = RecordHelper.FormatRecordFive(this.weaponActiviy);
        var weaponDail = RecordHelper.FormatRecordFive(this.weaponDaily);
        var roleDail = RecordHelper.FormatRecordFive(this.roleDaily);
        var allData = roleActivity.Concat(weaponActiviy).Concat(roleDaily).Concat(weaponDaily);
        this.StarPipeDatas.Clear();

        #region 星级占比
        StarPipeDatas.Add(
            new PieData()
            {
                Name = "3星",
                Offset = 0,
                Values = [allData.Where(x => x.QualityLevel == 3).Count()],
            }
        );
        StarPipeDatas.Add(
            new PieData()
            {
                Name = "4星",
                Offset = 0,
                Values = [allData.Where(x => x.QualityLevel == 4).Count()],
            }
        );
        StarPipeDatas.Add(
            new PieData()
            {
                Name = "5星",
                Offset = 0,
                Values = [allData.Where(x => x.QualityLevel == 5).Count()],
            }
        );
        #endregion

        #region 歪不歪
        if (roleActivity.Count > 0)
        {
            var roleRange = RecordHelper.FormatStartFive(
                roleActivity,
                RecordHelper.FormatFiveRoleStar(FiveGroup!)
            );
            var passValue = roleRange.Item1.Where(x => x.Item3 == false);
            var ngValue = roleRange.Item1.Where(x => x.Item3 == true);
            RangePipeDatas.Add(
                new PieData()
                {
                    Name = "歪了",
                    Offset = 0,
                    Values = [ngValue.Count()],
                }
            );
            RangePipeDatas.Add(
                new PieData()
                {
                    Name = "中了",
                    Offset = 0,
                    Values = [passValue.Count()],
                }
            );
            this.ExpectedActivityRoleDeily = (long)(80 - roleRange.Item2);
            this.Guaranteed = Math.Round(RecordHelper.GetGuaranteedRange(roleRange.Item1), 2);
        }

        #endregion

        foreach (var item in ruleActiv)
        {
            if (
                FiveGroup
                    .Data.FiveGroupConfig.FiveMaps.Where(x => x.ItemId == item.Item1.ResourceId)
                    .Any()
            )
            {
                RoleActivityCount++;
            }
            else
            {
                RoleActivityCount2++;
            }
        }
        if (weaponActiv.Count != 0)
        {
            var weaponRange = RecordHelper.FormatStartFive(
                weaponDaily,
                RecordHelper.FormatFiveRoleStar(FiveGroup!)
            );
            this.WeaponActivityCount = weaponActiv.Count;
            ExpectedDailyWeaponDeily = (long)(80 - weaponRange.Item2);
        }
        #endregion
        if (roleDail.Count != 0)
        {
            var RoleRange = RecordHelper.FormatStartFive(
                roleDaily,
                RecordHelper.FormatFiveRoleStar(FiveGroup!)
            );
            ExpectedRoleDeily = (long)(80 - RoleRange.Item2);
            CurrentRoleDeily = (double)RoleRange.Item2;
        }
        if (weaponDail.Count != 0)
        {
            var weaponRange = RecordHelper.FormatStartFive(
                weaponActiviy,
                RecordHelper.FormatFiveRoleStar(FiveGroup!)
            );
            this.WeaponActivityCount = weaponActiv.Count;
            ExpectedActivityWeaponDeily = (long)(80 - weaponRange.Item2);
            CurrentWeaponDeily = (double)weaponRange.Item2;
        }
        LoadingVisibility = Visibility.Collapsed;
        DataVisibility = Visibility.Visible;
        this.SelectNavigationItem = null;
        this.SelectNavigationItem = this.RecordNavigationItems[0];
    }
}

public partial class PieData : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial double[] Values { get; set; }

    [ObservableProperty]
    public partial double Offset { get; set; }
    public Func<ChartPoint, string> Formatter { get; set; } =
        point =>
        {
            var pv = point.Coordinate.PrimaryValue;
            var sv = point.StackedValue!;

            return $"{Math.Round(pv / sv.Total * 100, 2)}%";
        };
}
