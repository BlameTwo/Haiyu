﻿using Haiyu.Services.DialogServices;

namespace Haiyu.ViewModel.DialogViewModels;

public sealed partial class GameResourceViewModel : DialogViewModelBase
{
    public GameResourceViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager
    )
        : base(dialogManager) { }

    public string ContextName { get; private set; }
    public IGameContext GameContext { get; private set; }
    public IViewFactorys ViewFactorys { get; }

    [ObservableProperty]
    public partial string GameFilesSize { get; set; }

    [ObservableProperty]
    public partial string GameProdSize { get; set; }

    [ObservableProperty]
    public partial bool DeleteGameResourceEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool DeleteProdGameResourceEnable { get; set; } = true;

    [ObservableProperty]
    public partial FileVersion Dlss { get; private set; }

    [ObservableProperty]
    public partial FileVersion DlssG { get; private set; }

    [ObservableProperty]
    public partial FileVersion Xess { get; private set; }

    internal void SetData(string contextName)
    {
        ContextName = contextName;
        this.GameContext = Instance.Service.GetRequiredKeyedService<IGameContext>(contextName);
    }

    [RelayCommand]
    async Task Loaded()
    {
        var result =  GameContext.GameLocalConfig.GetConfig(
            GameLocalSettingName.GameLauncherBassFolder
        );
        var prodFolder = GameContext.GameLocalConfig.GetConfig(
            GameLocalSettingName.ProdDownloadFolderPath
        );
        long gameSize = 0L;
        long prodSize = 0L;
        var files = Directory.GetFiles(result, "*", SearchOption.TopDirectoryOnly).ToList();
        await Task.Run(() =>
        {
            foreach (
                var item in Directory.GetDirectories(result, "*", SearchOption.TopDirectoryOnly)
            )
            {
                if (item == prodFolder)
                {
                    continue;
                }
                else
                {
                    var file = Directory.GetFiles(item, "*", SearchOption.AllDirectories);
                    files.AddRange(file);
                }
            }
            foreach (var item in files)
            {
                gameSize += new FileInfo(item).Length;
            }

            if (Directory.Exists(prodFolder))
            {
                var files = Directory.GetFiles(prodFolder, "*.*", SearchOption.AllDirectories);
                foreach (var item in files)
                {
                    prodSize += new FileInfo(item).Length;
                }
            }
        });
        GameFilesSize = $"{(gameSize / (1024.0 * 1024.0 * 1024.0)):F2} GB";
        GameProdSize = $"{(prodSize / (1024.0 * 1024.0 * 1024.0)):F2} GB";
        if (gameSize == 0)
        {
            this.DeleteGameResourceEnable = false;
        }
        if (prodSize == 0)
        {
            this.DeleteProdGameResourceEnable = false;
        }

        //this.Dlss = await GameContext.GetLocalDLSSAsync();
        //this.DlssG = await GameContext.GetLocalDLSSGenerateAsync();
        //this.Xess = await GameContext.GetLocalXeSSGenerateAsync();
    }

    [RelayCommand]
    void SendDeleteGameResource()
    {
        WeakReferenceMessenger.Default.Send<DeleteGameResource>(new(true, ContextName));
        Close();
    }

    [RelayCommand]
    void SendDeleteProdGameResource()
    {
        WeakReferenceMessenger.Default.Send<DeleteGameResource>(new(false, ContextName));
        Close();
    }
}
