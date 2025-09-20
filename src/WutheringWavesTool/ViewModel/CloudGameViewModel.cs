using LiveChartsCore.Defaults;
using System.Globalization;
using Waves.Api.Models.CloudGame;
using Windows.Devices.Bluetooth.Advertisement;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel;

public partial class CloudGameViewModel : ViewModelBase
{
    private List<Datum> cacheItems;

    public CloudGameViewModel(
        ICloudGameService cloudGameService,
        ITipShow tipShow,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager
    )
    {
        CloudGameService = cloudGameService;
        TipShow = tipShow;
        DialogManager = dialogManager;
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

    [ObservableProperty]
    public partial long PageSize { get; set; } = 1;

    [ObservableProperty]
    public partial ObservableCollection<Datum> ResourceItems { get; set; } =
        new ObservableCollection<Datum>();

    [ObservableProperty]
    public partial ObservableCollection<LoginData> Users { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<CloudGameNavigationItem> NavigationItems { get; set; } =
        new ObservableCollection<CloudGameNavigationItem>()
        {
            new CloudGameNavigationItem() { DisplayName = "唤取记录", Tag = "record" },
            new CloudGameNavigationItem() { DisplayName = "唤取分析", Tag = "analysis" },
        };

    [ObservableProperty]
    public partial ObservableCollection<GameRecordNavigationItem> RecordNavigationItems { get; set; } =
        new ObservableCollection<GameRecordNavigationItem>()
        {
            new GameRecordNavigationItem() { Id = 1, DisplayName = "角色活动唤取" },
            new GameRecordNavigationItem() { Id = 2, DisplayName = "武器活动唤取" },
            new GameRecordNavigationItem() { Id = 3, DisplayName = "角色常驻唤取" },
            new GameRecordNavigationItem() { Id = 4, DisplayName = "武器常驻唤取" },
            new GameRecordNavigationItem() { Id = 5, DisplayName = "新手唤取" },
            new GameRecordNavigationItem() { Id = 6, DisplayName = "新手自选唤取" },
            new GameRecordNavigationItem() { Id = 7, DisplayName = "感恩定向唤取" },
            new GameRecordNavigationItem() { Id = 8, DisplayName = "角色新旅唤取" },
            new GameRecordNavigationItem() { Id = 9, DisplayName = "武器新旅唤取" },
        };

    [ObservableProperty]
    public partial GameRecordNavigationItem SelectRecordType { get; set; }

    [ObservableProperty]
    public partial CloudGameNavigationItem SelectNavigation { get; set; }

    [ObservableProperty]
    public partial LoginData SelectedUser { get; set; }

    [ObservableProperty]
    public partial Visibility LoadVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility DataVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility NoLoginVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// 唤取记录隐藏
    /// </summary>
    [ObservableProperty]
    public partial Visibility RecordVisibility { get; set; } = Visibility.Collapsed;

    /// <summary>
    /// 唤取分析隐藏
    /// </summary>
    [ObservableProperty]
    public partial Visibility AnalysisVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial bool IsLoginUser { get; set; } = true;

    [ObservableProperty]
    public partial ObservableCollection<DateTimePoint> AllPoints { get; set; } = new();

    private long currentPage = 1;
    public long CurrentPage
    {
        get => currentPage;
        set => SetProperty(ref currentPage, value);
    }

    private long totalPages = 1;
    public long TotalPages
    {
        get => totalPages;
        set => SetProperty(ref totalPages, value);
    }

    partial void OnPageSizeChanged(long value)
    {
        if (value <= 0)
            PageSize = 10;
    }

    [RelayCommand]
    public async Task Loaded()
    {
        var users = await TryInvokeAsync(CloudGameService.ConfigManager.GetUsersAsync());
        if (users.Item1 != 0 || users.Item2.Count == 0)
        {
            TipShow.ShowMessage("获取本地用户失败", Symbol.Clear);
            NoLoginVisibility = Visibility.Visible;
            this.LoadVisibility = Visibility.Collapsed;
            this.DataVisibility = Visibility.Collapsed;
            this.IsLoginUser = false;
            return;
        }
        this.IsLoginUser = true;
        this.Users = users.Item2;
        this.SelectedUser = Users[0];
    }

    public Func<DateTime, string> Formatter { get; set; } =
        date => date.ToString("MMMM dd");

    async partial void OnSelectedUserChanged(LoginData value)
    {
        if (value == null)
            return;
        NoLoginVisibility = Visibility.Collapsed;
        this.LoadVisibility = Visibility.Visible;
        this.DataVisibility = Visibility.Collapsed;
        this.SelectNavigation = null;
        this.SelectRecordType = null;
        var result = await CloudGameService.OpenUserAsync(value);
        NoLoginVisibility = Visibility.Collapsed;
        this.LoadVisibility = Visibility.Collapsed;
        this.DataVisibility = Visibility.Visible;
        this.SelectNavigation = NavigationItems[0];
    }

    partial void OnSelectNavigationChanged(CloudGameNavigationItem value)
    {
        if (value == null)
            return;

        if (value.Tag == "record")
        {
            this.RecordVisibility = Visibility.Visible;
            this.AnalysisVisibility = Visibility.Collapsed;
            this.SelectRecordType = RecordNavigationItems[0];
        }
        if(value.Tag == "analysis")
        {
            this.RecordVisibility = Visibility.Collapsed;
            this.AnalysisVisibility = Visibility.Visible;
            this.SelectRecordType = null;
        }
    }

    async partial void OnSelectRecordTypeChanged(GameRecordNavigationItem value)
    {
        if (value == null || SelectedUser == null)
            return;
        var url = await TryInvokeAsync(CloudGameService.GetRecordAsync());
        if (url.Item2.Code == 315)
        {
            TipShow.ShowMessage("登陆状态失效，请直接重新添加账号", Symbol.Clear);
            return;
        }
        if(url.Item2.Code == 5)
        {

            TipShow.ShowMessage("请求频繁，请稍等5s-10s", Symbol.Clear);
            return;
        }
        var resource = await TryInvokeAsync(
            CloudGameService.GetGameRecordResource(
                url.Item2.Data.RecordId,
                url.Item2.Data.PlayerId.ToString(),
                value.Id
            )
        );
        this.cacheItems = resource.Item2.Data;
        this.PageSize = 8;
        this.CurrentPage = 1;
        UpdatePageCount();
        LoadPageItems();
        var result = resource.Item2.Data
            .GroupBy(item =>
            {
                DateTime parsedDate;
                if (DateTime.TryParseExact(item.Time, "yyyy-MM-dd HH:mm:ss",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out parsedDate))
                {
                    return parsedDate.Date;
                }
                return (DateTime?)null; // 无法解析的日期返回 null，不会参与分组
            })
            .Where(g => g.Key.HasValue) // 过滤掉无效日期
            .ToDictionary(
                g => g.Key.Value.ToString("yyyy-MM-dd"),
                g => g.Count()
            );
        foreach (var item in result)
        {

            this.AllPoints.Add(new DateTimePoint(DateTime.Parse(item.Key), item.Value));
        }
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
}
