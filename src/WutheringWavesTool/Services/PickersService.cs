
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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

    public async Task<StorageFile> GetFileSavePicker()
    {
        FileSavePicker picker = new FileSavePicker();
        picker.FileTypeChoices.Add("主程序", new List<string>() { ".exe" });
        picker.SuggestedFileName = "Wuthering Waves";

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ApplicationSetup.App.MainWindow);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        StorageFile file = await picker.PickSaveFileAsync();
        return file;
    }

    public async Task<StorageFolder> GetFolderPicker()
    {
        FolderPicker openPicker = new FolderPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ApplicationSetup.App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        return await openPicker.PickSingleFolderAsync();
    }
}
