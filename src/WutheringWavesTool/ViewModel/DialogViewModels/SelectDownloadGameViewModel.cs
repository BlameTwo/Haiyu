using WutheringWavesTool.Services.DialogServices;
using static WutheringWavesTool.Controls.LayeredProgressBar;

namespace WutheringWavesTool.ViewModel.DialogViewModels;

public partial class SelectDownloadGameViewModel : DialogViewModelBase
{
    public SelectDownloadGameViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IPickersService pickersService
    )
        : base(dialogManager)
    {
        MaxValue = 100;
        PickersService = pickersService;
    }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }
    public GameLauncherSource? Launcher { get; private set; }

    [ObservableProperty]
    public partial bool ShowBar { get; set; } = false;

    [ObservableProperty]
    public partial string UpdateBefore { get; set; }

    [ObservableProperty]
    public partial string UpdateAfter { get; set; }

    [ObservableProperty]
    public partial string TipMessage { get; set; }

    public bool IsRead { get; private set; }

    [ObservableProperty]
    public partial ObservableCollection<LayerData> BarValues { get; set; }

    [ObservableProperty]
    public partial double MaxValue { get; set; }

    [ObservableProperty]
    public partial bool IsDownload { get; set; }

    [ObservableProperty]
    public partial string FolderPath { get; set; }

    public bool GetIsDownload() => IsDownload;

    partial void OnIsDownloadChanged(bool value)
    {
        StartDownloadCommand.NotifyCanExecuteChanged();
    }

    public IPickersService PickersService { get; }
    public IGameContext GameContext { get; private set; }

    [RelayCommand]
    async Task SelectFolder()
    {
        this.IsLoading = true;
        var folder = await PickersService.GetFileSavePicker();
        if (folder == null)
            return;
        this.FolderPath = System.IO.Path.GetDirectoryName(folder.Path)!;
        await folder.DeleteAsync();
        string? rootPath = Path.GetPathRoot(this.FolderPath);
        DriveInfo? selectedDrive = DriveInfo
            .GetDrives()
            .FirstOrDefault(drive =>
                drive.Name.Equals(rootPath, StringComparison.OrdinalIgnoreCase)
            );
        if (selectedDrive == null)
            return;
        double totalSizeMB = (double)selectedDrive.TotalSize / (1024 * 1024 * 1024);
        double freeSpaceMB = (double)selectedDrive.TotalFreeSpace / (1024 * 1024 * 1024);
        double usedSpaceMB = totalSizeMB - freeSpaceMB;
        MaxValue = totalSizeMB;
        ShowBar = true;
        this.IsLoading = false;
        Launcher = await this.GameContext.GetGameLauncherSourceAsync(this.CTS.Token);
        if (Launcher == null)
        {
            TipMessage = "数据拉取失败";
        }
        var updateSize = usedSpaceMB + Launcher.ResourceDefault.Config.Size / 1024 / 1024 / 1024;
        this.BarValues = new ObservableCollection<LayerData>()
        {
            new LayerData()
            {
                Label = "总容量",
                Color = new SolidColorBrush(Colors.LightGreen),
                Value = totalSizeMB,
            },
            new LayerData()
            {
                Label = "已用容量",
                Color = new SolidColorBrush(Colors.Purple),
                Value = usedSpaceMB,
            },
            new LayerData()
            {
                Label = "下载后增量",
                Color = new SolidColorBrush(Colors.Red),
                Value = updateSize,
            },
        };
        UpdateBefore = $"{Math.Round(usedSpaceMB, 2)}GB";
        UpdateAfter = $"{Math.Round(updateSize, 2)}GB";
        if (updateSize > totalSizeMB)
        {
            TipMessage = "空间不足，请清理一些文件进行下载";

            IsDownload = false;
        }
        else
        {
            TipMessage =
                $"本次更新大小约为{Launcher.ResourceDefault.Config.Size / 1024 / 1024 / 1024}GB";
            IsDownload = true;
        }
    }

    [RelayCommand(CanExecute = nameof(GetIsDownload))]
    void StartDownload()
    {
        this.Result = ContentDialogResult.Primary;
        this.Close();
    }

    internal void SetData(Type type)
    {
        var name = type.Name;
        this.GameContext = Instance.Service.GetRequiredKeyedService<IGameContext>(name);
    }
}
