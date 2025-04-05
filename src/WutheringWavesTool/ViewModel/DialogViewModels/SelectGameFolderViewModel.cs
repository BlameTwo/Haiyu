﻿using System.Collections.Concurrent;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel.DialogViewModels;

public sealed partial class SelectGameFolderViewModel : DialogViewModelBase
{
    public SelectGameFolderViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IPickersService pickersService
    )
        : base(dialogManager)
    {
        PickersService = pickersService;
    }

    public IGameContext GameContext { get; private set; }

    [ObservableProperty]
    public partial string ExePath { get; set; }

    [ObservableProperty]
    public partial string TipMessage { get; set; } = "选择目标程序，以查看驱动器详情";

    [ObservableProperty]
    public partial bool IsVerify { get; set; }

    public bool GetIsVerify() => IsVerify;

    [ObservableProperty]
    public partial ObservableCollection<LayerData> BarValues { get; set; }

    [ObservableProperty]
    public partial double MaxValue { get; set; }
    public IPickersService PickersService { get; }
    public GameLauncherSource Launcher { get; internal set; }

    [RelayCommand]
    async Task SelectGameProgram()
    {
        var exe = await PickersService.GetFileOpenPicker([".exe"]);
        if (exe == null)
            return;
        if (!exe.Name.StartsWith("Wuthering Waves"))
        {
            TipMessage = "无效地址";
            return;
        }
        this.ExePath = exe.Path;
        var folderPath = Path.GetDirectoryName(ExePath);
        var directoryInfo = new DirectoryInfo(folderPath);
        var folderSizeBytes = await CalculateFolderSizeAsync(directoryInfo);
        var folderSizeGB = BytesToGigabytes(folderSizeBytes);
        var rootPath = Path.GetPathRoot(ExePath);
        var driveInfo = GetDriveInfo(rootPath);

        if (driveInfo == null)
        {
            TipMessage = $"无法找到对应驱动器: {rootPath}";
            return;
        }

        Launcher = await this.GameContext.GetGameLauncherSourceAsync(this.CTS.Token);
        if (Launcher == null)
        {
            TipMessage = $"游戏数据拉取失败";
            return;
        }

        var totalSpaceGB = BytesToGigabytes(driveInfo.TotalSize);
        var freeSpaceGB = BytesToGigabytes(driveInfo.TotalFreeSpace);
        this.MaxValue = totalSpaceGB;
        this.BarValues = new ObservableCollection<LayerData>(
            [
                new LayerData()
                {
                    Label = "总容量",
                    Color = new SolidColorBrush(Colors.LightGreen),
                    Value = totalSpaceGB,
                },
                new LayerData()
                {
                    Label = "当前游戏文件夹容量",
                    Color = new SolidColorBrush(Colors.Purple),
                    Value = totalSpaceGB - freeSpaceGB,
                },
                new LayerData()
                {
                    Label = "占用容量",
                    Color = new SolidColorBrush(Colors.MediumPurple),
                    Value = totalSpaceGB - freeSpaceGB - folderSizeGB,
                },
            ]
        );
        IsVerify = true;
    }

    [RelayCommand]
    void StartVerify()
    {
        this.Result = ContentDialogResult.Primary;
        this.DialogManager.CloseDialog();
    }

    partial void OnIsVerifyChanged(bool value)
    {
        this.StartVerifyCommand.NotifyCanExecuteChanged();
    }

    private async Task<long> CalculateFolderSizeAsync(DirectoryInfo directory)
    {
        long totalSize = 0;
        var files = GetAccessibleFiles(directory);
        await Parallel.ForEachAsync(
            files,
            async (file, ct) =>
            {
                try
                {
                    Interlocked.Add(ref totalSize, file.Length);
                }
                catch (FileNotFoundException) { }
                await Task.CompletedTask;
            }
        );
        var subdirs = GetAccessibleDirectories(directory);
        await Parallel.ForEachAsync(
            subdirs,
            async (subdir, ct) =>
            {
                var size = await CalculateFolderSizeAsync(subdir);
                Interlocked.Add(ref totalSize, size);
            }
        );

        return totalSize;
    }

    private FileInfo[] GetAccessibleFiles(DirectoryInfo dir)
    {
        try
        {
            return dir.GetFiles();
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<FileInfo>();
        }
    }

    private DirectoryInfo[] GetAccessibleDirectories(DirectoryInfo dir)
    {
        try
        {
            return dir.GetDirectories();
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<DirectoryInfo>();
        }
    }

    private DriveInfo? GetDriveInfo(string rootPath)
    {
        return DriveInfo
            .GetDrives()
            .FirstOrDefault(d => d.Name.Equals(rootPath, StringComparison.OrdinalIgnoreCase));
    }

    private double BytesToGigabytes(long bytes) => bytes / 1024d / 1024 / 1024;

    internal void SetData(Type type)
    {
        var name = type.Name;
        this.GameContext = Instance.Service.GetRequiredKeyedService<IGameContext>(name);
    }
}
