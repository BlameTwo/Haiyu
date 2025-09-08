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
        };

    [ObservableProperty]
    public partial ObservableCollection<GameRecordNavigationItem> RecordNavigationItems { get; set; } =
        new ObservableCollection<GameRecordNavigationItem>()
        {
            new GameRecordNavigationItem(){ Id = 1, DisplayName = "角色活动唤取" },
            new GameRecordNavigationItem(){ Id = 2, DisplayName = "武器活动唤取" },
            new GameRecordNavigationItem(){ Id = 3, DisplayName = "角色常驻唤取" },
            new GameRecordNavigationItem(){ Id = 4, DisplayName = "武器常驻唤取" },
            new GameRecordNavigationItem(){ Id = 5, DisplayName = "新手唤取" },
            new GameRecordNavigationItem(){ Id = 6, DisplayName = "新手自选唤取" },
            new GameRecordNavigationItem(){ Id = 7, DisplayName = "感恩定向唤取" },
            new GameRecordNavigationItem(){ Id = 8, DisplayName = "角色新旅唤取" },
            new GameRecordNavigationItem(){ Id = 9, DisplayName = "武器新旅唤取" },
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

    // Pagination related properties (use distinct names to avoid conflicts with source-generator)
    private long currentPage = 1; // 当前页，从1开始
    public long CurrentPage
    {
        get => currentPage;
        set => SetProperty(ref currentPage, value);
    }

    private long totalPages = 1; // 总页数
    public long TotalPages
    {
        get => totalPages;
        set => SetProperty(ref totalPages, value);
    }

    // Ensure default page size is 10 items per page
    partial void OnPageSizeChanged(long value)
    {
        if (value <= 0)
            PageSize = 10;
    }

    [RelayCommand]
    public async Task Loaded()
    {
        // 请在当前代码中写入获取本地User的逻辑
        var users = await TryInvokeAsync(CloudGameService.ConfigManager.GetUsersAsync());
        if (users.Item1 != 0)
        {
            TipShow.ShowMessage("获取本地用户失败", Symbol.Clear);
        }
        this.Users = users.Item2;
        this.SelectedUser = Users[0];
    }

    async partial void OnSelectedUserChanged(LoginData value)
    {
        if (value == null)
            return;
        this.LoadVisibility = Visibility.Visible;
        this.DataVisibility = Visibility.Collapsed;
        this.SelectNavigation = null;
        this.SelectRecordType = null;
        var result = await CloudGameService.OpenUserAsync(value);
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
           this.SelectRecordType = RecordNavigationItems[0];
        }
    }

    async partial void OnSelectRecordTypeChanged(GameRecordNavigationItem value)
    {
        if (value == null || SelectedUser == null)
            return;
        var url = await TryInvokeAsync(CloudGameService.GetRecordAsync());
        var resource = await TryInvokeAsync(
            CloudGameService.GetGameRecordResource(
                url.Item2.Data.RecordId,
                url.Item2.Data.PlayerId.ToString(),value.Id
            )
        );
        this.cacheItems = resource.Item2.Data;
        this.PageSize = 10;
        this.CurrentPage = 1;
        UpdatePageCount();
        LoadPageItems();
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
