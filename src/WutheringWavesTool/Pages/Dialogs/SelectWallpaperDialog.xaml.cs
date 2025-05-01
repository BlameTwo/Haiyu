namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class SelectWallpaperDialog : ContentDialog, IDialog
{
    public SelectWallpaperDialog(SelectWallpaperViewModel viewModel)
    {
        this.InitializeComponent();
        ViewModel = viewModel;
        this.RequestedTheme = ElementTheme.Dark;
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
