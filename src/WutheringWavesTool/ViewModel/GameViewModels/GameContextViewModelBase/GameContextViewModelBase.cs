﻿

namespace Haiyu.ViewModel.GameViewModels
{
    public abstract partial class GameContextViewModelBase : ViewModelBase, IDisposable
    {
        public IGameContext GameContext { get; }
        public IDialogManager DialogManager { get; }
        public IAppContext<App> AppContext { get; }
        public ITipShow TipShow { get; }
        public IWallpaperService WallpaperService { get; }

        protected GameContextViewModelBase(
            IGameContext gameContext,
            IDialogManager dialogManager,
            IAppContext<App> appContext,
            ITipShow tipShow
        )
        {
            GameContext = gameContext;
            DialogManager = dialogManager;
            AppContext = appContext;
            TipShow = tipShow;
            WallpaperService = Instance.GetService<IWallpaperService>();
            GameContext.GameContextOutput += GameContext_GameContextOutput;
            this.AppContext.WallpaperService.WallpaperPletteChanged +=
                WallpaperService_WallpaperPletteChanged;
            this.StressShadowColor = AppContext.StressShadowColor;
        }

        private void WallpaperService_WallpaperPletteChanged(object sender, PletteArgs args)
        {
            if (args.Background == null || args.Forground == null || args.Shadow == null)
                return;
            this.StressShadowColor = args.Background.Value;
        }

        [ObservableProperty]
        public partial Color StressShadowColor { get; set; }

        #region MyRegion

        /// <summary>
        /// 选择下载路径显示
        /// </summary>
        [ObservableProperty]
        public partial Visibility GameInstallBthVisibility { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// 定位游戏路径显示
        /// </summary>
        [ObservableProperty]
        public partial Visibility GameInputFolderBthVisibility { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// 游戏下载中
        /// </summary>
        [ObservableProperty]
        public partial Visibility GameDownloadingBthVisibility { get; set; } = Visibility.Collapsed;

        [ObservableProperty]
        public partial Visibility GameLauncherBthVisibility { get; set; } = Visibility.Collapsed;

        [ObservableProperty]
        public partial string LauncherIcon { get; set; }

        [ObservableProperty]
        public partial ImageSource VersionLogo { get; set; }

        [ObservableProperty]
        public partial string LauncheContent { get; set; }

        [ObservableProperty]
        public partial string DisplayVersion { get; set; }
        #endregion

        [ObservableProperty]
        public partial string PauseIcon 
        { 
            get; 
            set; 
        }

        [ObservableProperty]
        public partial bool PauseStartEnable { get; set; } = true;

        [ObservableProperty]
        public partial string BottomBarContent { get; set; }



        [ObservableProperty]
        public partial bool EnableStartGameBth { get; set; } = false;

        /// <summary>
        /// 按钮类型,1为安装游戏,2为下载游戏,3为开始游戏,4为准备更新,5为游戏中
        /// </summary>
        private int _bthType = 0;
        private bool disposedValue;

        [RelayCommand]
        async Task Loaded()
        {
            try
            {
                var status = await this.GameContext.GetGameContextStatusAsync(this.CTS.Token);
                if (!status.IsGameExists && !status.IsGameInstalled)
                {
                    ShowSelectInstallBth();
                }
                if (status.IsGameExists && !status.IsGameInstalled && !status.IsLauncher)
                {
                    ShowGameDownloadBth();
                }
                else if (!status.IsAction && status.IsGameExists && status.IsGameInstalled)
                {
                    ShowGameLauncherBth(status.IsUpdate, status.DisplayVersion, status.Gameing);
                }
                if (status.IsGameExists && (status.IsPause || status.IsAction))
                {
                    if (status.IsAction && status.IsPause)
                    {
                        this.BottomBarContent = "下载已经暂停";
                        this.PauseIcon = "\uE896";
                    }
                    else
                    {
                        this.PauseIcon = "\uE769";
                    }
                    ShowGameDownloadingBth();
                }
                if (status.IsGameExists && status.IsGameInstalled && !status.IsPause && status.IsAction)
                {
                    this.PauseIcon = "\uE769";
                }
                if (status.IsGameExists && status.IsGameInstalled && status.IsPause && status.IsAction)
                {
                    this.PauseIcon = "\uE768";
                }
                var index = await this.GameContext.GetDefaultLauncherValue(this.CTS.Token);
                var background = await this.GameContext.GetLauncherBackgroundDataAsync(index.FunctionCode.Background, this.CTS.Token);
                WallpaperService.SetWallpaperForUrl(background.FirstFrameImage);
                this.VersionLogo = new BitmapImage(new(background.Slogan));
                await LoadAfter();
            }
            catch (Exception ex)
            {
                TipShow.ShowMessage(ex.Message, Symbol.Clear);
            }
        }

        private void ShowGameLauncherBth(bool isUpdate, string version, bool gameing)
        {
            GameInputFolderBthVisibility = Visibility.Collapsed;
            GameInstallBthVisibility = Visibility.Collapsed;
            GameDownloadingBthVisibility = Visibility.Collapsed;
            GameLauncherBthVisibility = Visibility.Visible;
            if (isUpdate)
            {
                _bthType = 4;
                this.CurrentProgressValue = 0;
                this.MaxProgressValue = 0;
                BottomBarContent = "游戏有更新，请点击右侧按钮进行更新";
                LauncheContent = "更新游戏";
                DisplayVersion = version;
                EnableStartGameBth = true;
                LauncherIcon = "\uE898";
            }
            else
            {
                if (gameing)
                {
                    _bthType = 5;
                    this.CurrentProgressValue = 0;
                    this.MaxProgressValue = 0;
                    BottomBarContent = "";
                    LauncheContent = "游戏中";
                    EnableStartGameBth = false;
                    DisplayVersion = version;
                    LauncherIcon = "\uE71A";
                }
                else
                {
                    _bthType = 3;
                    this.CurrentProgressValue = 0;
                    this.MaxProgressValue = 0;
                    BottomBarContent = "游戏准备就绪";
                    LauncheContent = "进入游戏";
                    EnableStartGameBth = true;
                    DisplayVersion = version;
                    LauncherIcon = "\uE7FC";
                }
            }
        }

        [RelayCommand]
        async Task ShowSelectInstallFolder()
        {
            if (_bthType == 1)
            {
                var result = await DialogManager.ShowSelectDownloadFolderAsync(
                    this.GameContext.ContextType
                );
                if (result.Result == ContentDialogResult.None)
                {
                    return;
                }
                await this.GameContext.StartDownloadTaskAsync(
                    result.InstallFolder,
                    result.Launcher
                );
            }
            else
            {
                var launcher = await GameContext.GetGameLauncherSourceAsync(this.CTS.Token);
                await this.GameContext.StartDownloadTaskAsync(
                    GameContext.GameLocalConfig.GetConfig(
                        GameLocalSettingName.GameLauncherBassFolder
                    ),
                    launcher
                );
            }
        }

        [RelayCommand]
        async Task ShowSelectGameFolder()
        {
            if (_bthType == 1)
            {
                var result = await DialogManager.ShowSelectGameFolderAsync(
                    this.GameContext.ContextType
                );
                if (result.Result == ContentDialogResult.None)
                {
                    return;
                }
                await this.GameContext.StartDownloadTaskAsync(
                    result.InstallFolder,
                    result.Launcher
                );
            }
            else
            {
                var launcher = await GameContext.GetGameLauncherSourceAsync(this.CTS.Token);
                await this.GameContext.StartDownloadTaskAsync(
                    GameContext.GameLocalConfig.GetConfig(
                        GameLocalSettingName.GameLauncherBassFolder
                    ),
                    launcher
                );
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        private void ShowSelectInstallBth()
        {
            _bthType = 1;
            GameInputFolderBthVisibility = Visibility.Visible;
            GameInstallBthVisibility = Visibility.Visible;
            GameDownloadingBthVisibility = Visibility.Collapsed;
            GameLauncherBthVisibility = Visibility.Collapsed;
            BottomBarContent = "游戏文件不存在，请找到窗口右下角选择游戏下载路径或定位游戏";
        }

        [RelayCommand]
        async Task RepirGame()
        {
            await GameContext.RepirGameAsync();
        }

        [RelayCommand]
        async Task ShowGameResource()
        {
            await DialogManager.ShowGameResourceDialogAsync(this.GameContext.ContextName);
        }

        [RelayCommand]
        async Task DeleteGameResource()
        {
            await GameContext.DeleteResourceAsync();
        }

        private void ShowGameDownloadingBth()
        {
            _bthType = 2;
            if (GameDownloadingBthVisibility == Visibility.Visible)
                return;
            this.PauseIcon = "\uE769";
            GameInputFolderBthVisibility = Visibility.Collapsed;
            GameInstallBthVisibility = Visibility.Collapsed;
            GameLauncherBthVisibility = Visibility.Collapsed;
            GameDownloadingBthVisibility = Visibility.Visible;
        }

        /// <summary>
        /// 显示继续下载
        /// </summary>
        private void ShowGameDownloadBth()
        {
            _bthType = 2;
            GameInputFolderBthVisibility = Visibility.Collapsed;
            GameInstallBthVisibility = Visibility.Visible;
            GameDownloadingBthVisibility = Visibility.Collapsed;
            GameLauncherBthVisibility = Visibility.Collapsed;
            BottomBarContent = "请点击右下角继续更新游戏";
        }

        public abstract Task LoadAfter();

        public abstract void DisposeAfter();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GameContext.GameContextOutput -= GameContext_GameContextOutput;
                    this.AppContext.WallpaperService.WallpaperPletteChanged -=
                        WallpaperService_WallpaperPletteChanged;
                    DisposeAfter();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
