using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel.DialogViewModels;

public sealed partial class SelectWallpaperViewModel : DialogViewModelBase
{
    public SelectWallpaperViewModel(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IPickersService pickersService,
        IAppContext<App> appContext
    )
        : base(dialogManager)
    {
        PickersService = pickersService;
        AppContext = appContext;
    }

    public IPickersService PickersService { get; }
    public IAppContext<App> AppContext { get; }

    [ObservableProperty]
    public partial ObservableCollection<WallpaperModel> Images { get; set; } = new();

    private async Task InitWallpaperAsync()
    {
        try
        {
            await foreach (var a in AppContext.WallpaperService.GetFilesAsync(this.CTS.Token))
            {
                if (this.Images == null)
                    Images = [];
                this.Images.Add(a);
            }
        }
        catch (TaskCanceledException) { }
    }

    [RelayCommand]
    async Task Loaded()
    {
        await InitWallpaperAsync();
    }

    public override void AfterClose()
    {
        foreach (var item in Images)
        {
            item.Image = null;
        }
        this.Images.Clear();
        this.Images = null;
        base.AfterClose();
    }

    [RelayCommand]
    async Task SelectWallpaper()
    {
        var file = await PickersService.GetFileOpenPicker([".jpg", ".png"]);
        if (file == null)
            return;
        await AppContext.WallpaperService.SetWallpaperAsync(file.Path);
    }
}
