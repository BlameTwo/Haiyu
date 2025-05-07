using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel.DialogViewModels;

public partial class SelectDownloadGameViewModel : DialogViewModelBase
{
    public SelectDownloadGameViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IPickersService pickersService
    )
        : base(dialogManager)
    {
        PickersService = pickersService;
    }

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

    partial void OnIsDownloadChanged(bool value) { }

    public IPickersService PickersService { get; }
    public IGameContext GameContext { get; private set; }

    [RelayCommand]
    async Task SelectFolder()
    {
        var result = await PickersService.GetFolderPicker();
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
