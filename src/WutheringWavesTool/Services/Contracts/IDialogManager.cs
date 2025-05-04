using WutheringWavesTool.Models.Dialogs;

namespace WutheringWavesTool.Services.Contracts;

public interface IDialogManager
{
    public XamlRoot Root { get; }
    public void SetDialog(ContentDialog contentDialog);
    public void RegisterRoot(XamlRoot root);
    public Task ShowLoginDialogAsync();
    public Task<ContentDialogResult> ShowBindGameDataAsync(string name);
    public Task ShowGameResourceDialogAsync(string contextName);
    public Task<Result> GetDialogResultAsync<T, Result>(object? data)
        where T : ContentDialog, IResultDialog<Result>, new()
        where Result : new();

    public Task<SelectDownloadFolderResult> ShowSelectInstallFolderAsync(Type type);

    public Task<SelectDownloadFolderResult> ShowSelectGameFolderAsync(Type type);

    public Task<CloseWindowResult> ShowCloseWindowResult();
    public void CloseDialog();
    Task ShowWallpaperDialogAsync();
}
