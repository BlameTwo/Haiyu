using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.Shell.Common;
using FolderPicker = Windows.Storage.Pickers.FolderPicker;

namespace WutheringWavesTool.Services;

public class PickersService : IPickersService
{
    public PickersService(IAppContext<App> applicationSetup)
    {
        ApplicationSetup = applicationSetup;
    }

    public IAppContext<App> ApplicationSetup { get; }

    public async Task<StorageFile> GetFileOpenPicker(List<string> extention)
    {
        FileOpenPicker openPicker = new FileOpenPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ApplicationSetup.App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        foreach (var item in extention)
        {
            openPicker.FileTypeFilter.Add(item);
        }
        return await openPicker.PickSingleFileAsync();
    }

    public FileSavePicker GetFileSavePicker()
    {
        throw new NotImplementedException();
    }

    public async Task<StorageFolder> GetFolderPicker()
    {
        FolderPicker openPicker = new FolderPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ApplicationSetup.App.MainWindow);
        DevWinUI.FolderPicker folderPicker = new DevWinUI.FolderPicker(hWnd);
        return await folderPicker.PickSingleFolderAsync();
    }
}
