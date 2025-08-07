﻿using System.Threading.Tasks;
using Waves.Api.Models;
using Windows.System;
using WutheringWavesTool.Models.Dialogs;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.Pages.Dialogs
{
    public sealed partial class SelectDownoadGameDialog
        : ContentDialog,
            IResultDialog<SelectDownloadFolderResult>
    {
        public SelectDownoadGameDialog()
        {
            InitializeComponent();
            this.DialogManager = Instance.Service.GetRequiredKeyedService<IDialogManager>(
                nameof(MainDialogService)
            );
            this.Pickers = Instance.Service.GetRequiredService<IPickersService>();
        }

        SelectDownloadFolderResult downloadResult = null;
        ContentDialogResult clickBth = ContentDialogResult.None;
        public IGameContext GameContext { get; private set; }
        public IDialogManager DialogManager { get; }
        public IPickersService Pickers { get; }
        public GameLauncherSource Launcher { get; private set; }

        public SelectDownloadFolderResult GetResult()
        {
            return this.downloadResult;
        }

        public void SetData(object data)
        {
            if (data is Type type)
            {
                var name = type.Name;
                this.GameContext = Instance.Service.GetRequiredKeyedService<IGameContext>(name);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            downloadResult = new SelectDownloadFolderResult()
            {
                InstallFolder = this.folderPath.Text,
                Result = clickBth,
            };
            this.DialogManager.CloseDialog();
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            var launcher = await this.GameContext.GetGameLauncherSourceAsync();
            if (launcher == null)
            {
                return;
            }
            this.clickBth = ContentDialogResult.Primary;
            downloadResult = new SelectDownloadFolderResult()
            {
                InstallFolder = this.folderPath.Text,
                Result = clickBth,
                Launcher = launcher,
            };
            this.DialogManager.CloseDialog();
        }

        private async void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderPath = await Pickers.GetFolderPicker();
            if (folderPath == null)
                return;
            if (!Directory.Exists(folderPath.Path))
            {
                return;
            }
            this.folderPath.Text = folderPath.Path;
            string? rootPath = Path.GetPathRoot(this.folderPath.Text);
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
            layered.MaxValue = totalSizeMB;
            layeredGrid.Visibility = Visibility.Visible;
            layerText.Visibility = Visibility.Collapsed;
            Launcher = await this.GameContext.GetGameLauncherSourceAsync();
            if (Launcher == null)
            {
                TipMessage.Text = "数据拉取失败";
                return;
            }
            var updateSize =
                usedSpaceMB + Launcher.ResourceDefault.Config.Size / 1024 / 1024 / 1024;
            this.layered.Values = new ObservableCollection<LayerData>()
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
            if (updateSize > totalSizeMB)
            {
                TipMessage.Text = "空间不足，请清理一些文件进行下载";
                download.Fill = new SolidColorBrush(Colors.Red);
                downloadBth.IsEnabled = false;
                return;
            }
            else
            {
                TipMessage.Text =
                    $"本次更新大小约为{Launcher.ResourceDefault.Config.Size / 1024 / 1024 / 1024}GB";
                downloadBth.IsEnabled = true;
                download.Fill = new SolidColorBrush(Colors.Green);
            }
        }
    }
}
