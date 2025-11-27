using System;
using System.Globalization;
using System.Linq;
using Haiyu.Contracts;
using Haiyu.Services.DialogServices;
using LiveChartsCore.Defaults;
using MemoryPack;
using Waves.Api.Models.CloudGame;

namespace Haiyu.ViewModel;

public partial class CloudGameViewModel : ViewModelBase
{
    private List<Datum> cacheItems;

    public CloudGameViewModel(
        ICloudGameService cloudGameService,
        ITipShow tipShow,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IViewFactorys viewFactorys
    )
    {
        CloudGameService = cloudGameService;
        TipShow = tipShow;
        DialogManager = dialogManager;
        ViewFactorys = viewFactorys;
        RegisterMananger();
    }

    private void RegisterMananger()
    {
        this.Messenger.Register<CloudLoginMessager>(this, CloudLoginMethod);
    }

    private async void CloudLoginMethod(object recipient, CloudLoginMessager message)
    {
        if (message == null || !message.Refresh)
            return;
        await this.Loaded();
    }

    public ICloudGameService CloudGameService { get; }
    public ITipShow TipShow { get; }
    public IDialogManager DialogManager { get; }
    public IViewFactorys ViewFactorys { get; }

    [ObservableProperty]
    public partial long PageSize { get; set; } = 1;

    [ObservableProperty]
    public partial ObservableCollection<Datum> ResourceItems { get; set; } =
        new ObservableCollection<Datum>();

    [ObservableProperty]
    public partial ObservableCollection<LoginData> Users { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<GameRecordNavigationItem> RecordNavigationItems { get; set; } =
        GameRecordNavigationItem.Default;

    [ObservableProperty]
    public partial GameRecordNavigationItem SelectRecordType { get; set; }

    [ObservableProperty]
    public partial LoginData SelectedUser { get; set; }

    [ObservableProperty]
    public partial Visibility LoadVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility DataVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility NoLoginVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial bool IsLoginUser { get; set; } = true;

    [ObservableProperty]
    public partial ObservableCollection<DateTimePoint> AllPoints { get; set; } = new();

    [ObservableProperty]
    public partial long CurrentPage { get; set; }

    [ObservableProperty]
    public partial long TotalPages { get; set; }

    partial void OnPageSizeChanged(long value)
    {
        if (value <= 0)
            PageSize = 10;
    }

    [RelayCommand]
    public async Task Loaded()
    {
        var users = await TryInvokeAsync(async () =>
            await CloudGameService.ConfigManager.GetUsersAsync(this.CTS.Token)
        );
        if (users.Item1 != 0 || users.Item2.Count == 0)
        {
            TipShow.ShowMessage("获取本地用户失败", Symbol.Clear);
            NoLoginVisibility = Visibility.Visible;
            this.LoadVisibility = Visibility.Collapsed;
            this.DataVisibility = Visibility.Collapsed;
            this.IsLoginUser = false;
            return;
        }
        this.Users = users.Item2;
        this.SelectedUser = Users[0];
        this.IsLoginUser = true;
        await SavePlayCardData();
    }

    [RelayCommand]
    public void ShowAnalysis()
    {
        ViewFactorys.ShowAnalysisRecord(this.SelectedUser).Show();
    }

    async partial void OnSelectedUserChanged(LoginData value)
    {
        if (value == null)
            return;
        NoLoginVisibility = Visibility.Collapsed;
        this.LoadVisibility = Visibility.Visible;
        this.DataVisibility = Visibility.Collapsed;
        this.SelectRecordType = null;
        var result = await CloudGameService.OpenUserAsync(value);
        NoLoginVisibility = Visibility.Collapsed;
        this.LoadVisibility = Visibility.Collapsed;
        this.DataVisibility = Visibility.Visible;
        this.SelectRecordType = RecordNavigationItems[0];
        if (!result.Item1)
        {
            TipShow.ShowMessage(result.Item2, Symbol.Clear);
            return;
        }
    }

    async partial void OnSelectRecordTypeChanged(GameRecordNavigationItem value)
    {
        if (value == null || SelectedUser == null)
            return;
        this.IsLoginUser = false;
        var url = await TryInvokeAsync(async () =>
            await CloudGameService.GetRecordAsync(this.CTS.Token)
        );
        if (url.Item2.Code == 315)
        {
            TipShow.ShowMessage("登陆状态失效，请直接重新添加账号", Symbol.Clear);
            return;
        }
        if (url.Item2.Code == 5)
        {
            TipShow.ShowMessage("请求频繁，请稍等5s-10s", Symbol.Clear);
            return;
        }
        var resource = await TryInvokeAsync(async () =>
            await CloudGameService.GetGameRecordResource(
                url.Item2.Data.RecordId,
                url.Item2.Data.PlayerId.ToString(),
                value.Id,
                this.CTS.Token
            )
        );
        if (resource.Result == null)
        {
            TipShow.ShowMessage("请求失败！", Symbol.Clear);
            return;
        }
        this.cacheItems = resource.Item2.Data;
        this.PageSize = 9;
        this.CurrentPage = 1;

        UpdatePageCount();
        LoadPageItems();

        this.IsLoginUser = true;
    }

    // 更新总页数
    private void UpdatePageCount()
    {
        if (cacheItems == null || cacheItems.Count == 0)
        {
            TotalPages = 1;
            return;
        }

        var pageSize = (int)(PageSize > 0 ? PageSize : 10);
        TotalPages = (cacheItems.Count + pageSize - 1) / pageSize;
    }

    // 从缓存中加载当前页的数据到 ResourceItems
    private void LoadPageItems()
    {
        ResourceItems.Clear();
        if (cacheItems == null || cacheItems.Count == 0)
            return;

        var pageSize = (int)(PageSize > 0 ? PageSize : 10);
        var pageIdx = (int)(CurrentPage <= 0 ? 1 : CurrentPage);
        var start = (pageIdx - 1) * pageSize;
        if (start >= cacheItems.Count)
            return;
        var page = cacheItems.Skip(start).Take(pageSize);
        foreach (var item in page)
            ResourceItems.Add(item);
    }

    [RelayCommand]
    public void NextPage()
    {
        if (cacheItems == null || cacheItems.Count == 0)
            return;

        UpdatePageCount();
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            LoadPageItems();
        }
    }

    [RelayCommand]
    public void PrevPage()
    {
        if (cacheItems == null || cacheItems.Count == 0)
            return;

        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadPageItems();
        }
    }

    [RelayCommand]
    public async Task ShowAdd()
    {
        await DialogManager.ShowWebGameDialogAsync();
    }

    public async Task SavePlayCardData()
    {
        try
        {
            this.LoadVisibility = Visibility.Visible;
            this.DataVisibility = Visibility.Collapsed;
            var FiveGroup = await RecordHelper.GetFiveGroupAsync();
            var AllRole = await RecordHelper.GetAllRoleAsync();
            var AllWeapon = await RecordHelper.GetAllWeaponAsync();
            var StartRole = RecordHelper.FormatFiveRoleStar(FiveGroup);
            var StartWeapons = RecordHelper.FormatFiveWeaponeRoleStar(FiveGroup);
            var isLogin =  await TryInvokeAsync(async () => await CloudGameService.OpenUserAsync(this.SelectedUser));
            if(isLogin.Result.Item1 == false)
            {
                TipShow.ShowMessage("登陆过期，请重新添加账号", Symbol.Clear);
                this.LoadVisibility = Visibility.Collapsed;
                this.DataVisibility = Visibility.Collapsed;
                return;
            }
            var url = await TryInvokeAsync(async () =>
                await CloudGameService.GetRecordAsync(this.CTS.Token)
            );
            #region 读取抽卡记录
            if(url.Result.Data != null)
            {
                Dictionary<int, IList<RecordCardItemWrapper>> @param =
                new Dictionary<int, IList<RecordCardItemWrapper>>();
                for (int i = 1; i < 10; i++)
                {
                    var player1 = await TryInvokeAsync(async () =>
                        await CloudGameService.GetGameRecordResource(
                            url.Item2.Data.RecordId,
                            url.Item2.Data.PlayerId.ToString(),
                            i,
                            this.CTS.Token
                        )
                    );
                    if (player1.Result == null)
                    {
                        TipShow.ShowMessage("数据拉取失败！", Symbol.Clear);
                        return;
                    }
                    var WeaponsActivity = player1
                        .Result.Data.Select(x => new RecordCardItemWrapper(x))
                        .ToList();
                    param.Add(i, WeaponsActivity);
                }
                var cache = new RecordCacheDetily()
                {
                    Name = this.SelectedUser.Username,
                    Time = DateTime.Now,
                    RoleActivityItems = param[1],
                    WeaponsActivityItems = param[2],
                    RoleResidentItems = param[3],
                    WeaponsResidentItems = param[4],
                    BeginnerItems = param[5],
                    BeginnerChoiceItems = param[6],
                    GratitudeOrientationItems = param[7],
                    RoleJourneyItems = param[8],
                    WeaponJourneyItems = param[9]
                };
                var datas = MemoryPackSerializer.Serialize<RecordCacheDetily>(
                    cache,
                    new MemoryPackSerializerOptions() { StringEncoding = StringEncoding.Utf8 }
                );
                var result = await RecordHelper.MargeRecordAsync(App.RecordFolder, cache)!;
                TipShow.ShowMessage($"抽卡合并，数据总量{result.Item2},二进制大小{result.Item1 / 1024}KB", Symbol.Accept);

                this.LoadVisibility = Visibility.Collapsed;
                this.DataVisibility = Visibility.Visible;
            }
            else
            {
                TipShow.ShowMessage($"{url.Message}", Symbol.Clear);
                this.LoadVisibility = Visibility.Collapsed;
                this.DataVisibility = Visibility.Collapsed;
            }
            
            #endregion
            
        }
        catch (Exception ex)
        {
            TipShow.ShowMessage(ex.Message, Symbol.Clear);
        }
        
    }
}
