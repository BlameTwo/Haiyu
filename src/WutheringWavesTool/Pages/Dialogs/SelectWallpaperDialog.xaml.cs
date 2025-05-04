namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class SelectWallpaperDialog : ContentDialog, IDialog
{
    public SelectWallpaperDialog(SelectWallpaperViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
        this.RequestedTheme =
            AppSettings.AppTheme == null ? ElementTheme.Default
            : AppSettings.AppTheme == "Dark" ? ElementTheme.Dark
            : AppSettings.AppTheme == "Light" ? ElementTheme.Light
            : ElementTheme.Default;
    }

    public SelectWallpaperViewModel ViewModel { get; }

    public void SetData(object data) { }

    private void ItemsView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
    {
        this.ViewModel.AppContext.WallpaperService.SetWallpaperAsync(
            (args.InvokedItem as WallpaperModel)!.FilePath
        );
    }
}
