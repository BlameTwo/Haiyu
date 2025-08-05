using WutheringWavesTool.Models.Dialogs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WutheringWavesTool.Services;

public abstract class DialogManager : IDialogManager
{
    ContentDialog _dialog = null;
    public XamlRoot Root { get; private set; }

    public void RegisterRoot(XamlRoot root)
    {
        this.Root = root;
    }

    public void SetDialog(ContentDialog contentDialog)
    {
        this._dialog = contentDialog;
    }

    public async Task ShowLoginDialogAsync() => await ShowDialogAsync<LoginDialog>();

    public async Task ShowGameResourceDialogAsync(string contextName)
    {
        var dialog = Instance.Service.GetRequiredService<GameResourceDialog>();
        dialog.SetData(contextName);
        dialog.XamlRoot = this.Root;
        this._dialog = dialog;
        await _dialog.ShowAsync();
    }

    public async Task<ContentDialogResult> ShowBindGameDataAsync(string name) =>
        await ShowDialogAsync<BindGameDataDialog>(name);

    public async Task ShowDialogAsync<T>()
        where T : ContentDialog, IDialog
    {
        if (_dialog != null)
            return;
        var dialog = Instance.Service.GetRequiredService<T>();
        dialog.XamlRoot = this.Root;
        this._dialog = dialog;
        var result = await _dialog.ShowAsync();
        _dialog = null;
    }

    public async Task<ContentDialogResult> ShowDialogAsync<T>(object data)
        where T : ContentDialog, IDialog
    {
        if (_dialog != null)
            return ContentDialogResult.None;
        var dialog = Instance.Service.GetRequiredService<T>();
        dialog.XamlRoot = this.Root;
        dialog.SetData(data);
        this._dialog = dialog;
        var result = await _dialog.ShowAsync();
        _dialog = null;
        return result;
    }

    public void CloseDialog()
    {
        if (_dialog == null)
            return;
        _dialog.Hide();
        GC.Collect();
    }

    public async Task ShowWallpaperDialogAsync()
    {
        await ShowDialogAsync<SelectWallpaperDialog>(null);
    }

    public async Task<Result> GetDialogResultAsync<T, Result>(object? data)
        where T : ContentDialog, IResultDialog<Result>, new()
        where Result : new()
    {
        if (_dialog != null)
            return new Result();
        var dialog = Instance.Service.GetRequiredService<T>();
        dialog.XamlRoot = this.Root;
        dialog.SetData(data);
        this._dialog = dialog;
        await _dialog.ShowAsync();
        var result = ((IResultDialog<Result>)_dialog).GetResult();
        _dialog = null;
        return result;
    }

    public async Task<SelectDownloadFolderResult> ShowSelectGameFolderAsync(Type type) =>
        await GetDialogResultAsync<SelectGameFolderDialog, SelectDownloadFolderResult>(type);

    public async Task<SelectDownloadFolderResult> ShowSelectDownloadFolderAsync(Type type) =>
        await GetDialogResultAsync<SelectDownoadGameDialog, SelectDownloadFolderResult>(type);

    public async Task<CloseWindowResult> ShowCloseWindowResult() =>
        await GetDialogResultAsync<CloseDialog, CloseWindowResult>(null);

    public async Task<QRScanResult> GetQRLoginResultAsync() => await GetDialogResultAsync<QRLoginDialog, QRScanResult>(null);
}
