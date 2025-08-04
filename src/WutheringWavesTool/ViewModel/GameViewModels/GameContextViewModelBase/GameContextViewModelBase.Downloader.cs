﻿namespace WutheringWavesTool.ViewModel.GameViewModels;

partial class GameContextViewModelBase
{
    [ObservableProperty]
    public partial double MaxProgressValue { get; set; }

    [ObservableProperty]
    public partial double CurrentProgressValue { get; set; }

    [ObservableProperty]
    public partial string DownloadSpeedValue { get; set; }

    private async Task GameContext_GameContextOutput(object sender, GameContextOutputArgs args)
    {
        await AppContext.TryInvokeAsync(async () =>
        {
            if (
                args.Type == Waves.Core.Models.Enums.GameContextActionType.Download
                || args.Type == Waves.Core.Models.Enums.GameContextActionType.Verify
                || args.Type == Waves.Core.Models.Enums.GameContextActionType.Decompress
            )
            {
                this.MaxProgressValue = args.TotalSize;
                this.CurrentProgressValue = args.CurrentSize;
                if (args.Type == Waves.Core.Models.Enums.GameContextActionType.Verify)
                {
                    if (args.IsAction && args.IsPause)
                    {
                        this.BottomBarContent = "下载已经暂停";
                    }
                    else
                    {
                        this.BottomBarContent =
                            $"校验速度:{Math.Round(args.VerifySpeed / 1024 / 1024, 2)}MB,剩余：{Math.Round((double)(args.TotalSize - args.CurrentSize) / 1024 / 1024 / 1024, 2)}GB";
                    }
                }
                else if (args.Type == Waves.Core.Models.Enums.GameContextActionType.Download)
                {
                    if (args.IsAction && args.IsPause)
                    {
                        this.BottomBarContent = "下载已经暂停";
                    }
                    else
                    {
                        this.BottomBarContent =
                            $"下载速度:{Math.Round(args.DownloadSpeed / 1024 / 1024, 2)}MB，剩余：{Math.Round((double)(args.TotalSize - args.CurrentSize) / 1024 / 1024 / 1024, 2)}GB";
                    }
                }
                else if(args.Type == Waves.Core.Models.Enums.GameContextActionType.Decompress)
                {
                    this.BottomBarContent =
                            $"已解压:{Math.Round((double)args.CurrentSize / 1024 / 1024/1024, 2)}GB,剩余:{Math.Round((double)(args.TotalSize - args.CurrentSize) / 1024 / 1024 / 1024, 2)}GB";
                }
                ShowGameDownloadingBth();
            }
            if (args.Type == Waves.Core.Models.Enums.GameContextActionType.DeleteFile)
            {
                ShowGameDownloadingBth();
                this.MaxProgressValue = args.FileTotal;
                this.CurrentProgressValue = args.CurrentFile;
                this.BottomBarContent = args.DeleteString;
            }
            if (args.Type == Waves.Core.Models.Enums.GameContextActionType.None)
            {
                this.CurrentProgressValue = 0;
                this.MaxProgressValue = 100;
                var status = await this.GameContext.GetGameContextStatusAsync(this.CTS.Token);
                if (!status.IsGameExists && !status.IsGameInstalled)
                {
                    ShowSelectInstallBth();
                }
                if (status.IsGameExists && !status.IsGameInstalled)
                {
                    ShowGameDownloadBth();
                }
                if (status.IsLauncher)
                {
                    ShowGameLauncherBth(status.IsUpdate, status.DisplayVersion, status.Gameing);
                }
                if (
                    status.IsGameExists
                    && !status.IsGameInstalled
                    && (status.IsPause || status.IsAction)
                )
                {
                    ShowGameDownloadingBth();
                    if (status.IsPause)
                    {
                        this.PauseIcon = "\uE768";
                    }
                    else
                    {
                        this.PauseIcon = "\uE769";
                    }
                }
            }
        });
    }

    [RelayCommand]
    async Task PauseDownloadTask()
    {
        var status = await this.GameContext.GetGameContextStatusAsync(this.CTS.Token);
        if (status.IsPause)
        {
            if (await this.GameContext.ResumeDownloadAsync())
            {
                this.BottomBarContent = "下载已恢复";
                this.PauseIcon = "\uE769";
            }
        }
        else
        {
            if (await this.GameContext.PauseDownloadAsync())
            {
                this.BottomBarContent = "下载已经暂停";
                this.PauseIcon = "\uE768";
            }
        }
    }

    [RelayCommand]
    async Task CancelDownloadTask()
    {
        await GameContext.StopDownloadAsync();
        var status = await GameContext.GetGameContextStatusAsync();
        if (!status.IsLauncher)
        {
            await this.GameContext.GameLocalConfig.SaveConfigAsync(
                GameLocalSettingName.GameLauncherBassFolder,
                ""
            );
            await this.GameContext.GameLocalConfig.SaveConfigAsync(
                GameLocalSettingName.GameLauncherBassProgram,
                ""
            );
            await this.GameContext.GameLocalConfig.SaveConfigAsync(
                GameLocalSettingName.LocalGameUpdateing,
                "False"
            );
        }
        await this.GameContext_GameContextOutput(
            this.GameContext,
            new GameContextOutputArgs()
            {
                Type = Waves.Core.Models.Enums.GameContextActionType.None,
            }
        );
    }

    [RelayCommand]
    async Task SetDownloadSpeedAsync()
    {
        if (int.TryParse(DownloadSpeedValue, out var result))
        {
            await GameContext.SetSpeedLimitAsync(result * 1024 * 1024);
        }
    }
}
