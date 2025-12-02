using Microsoft.Windows.Storage.Pickers;
namespace Haiyu.Services;

public class PickersService : IPickersService
{
    public PickersService(IAppContext<App> applicationSetup)
    {
        ApplicationSetup = applicationSetup;
    }

    public IAppContext<App> ApplicationSetup { get; }

    public async Task<PickFileResult> GetFileOpenPicker(List<string> extention)
    {
        FileOpenPicker openPicker = new FileOpenPicker(ApplicationSetup.App.MainWindow.AppWindow.Id);
        foreach (var item in extention)
        {
            openPicker.FileTypeFilter.Add(item);
        }
        return await openPicker.PickSingleFileAsync();
    }

    public async Task<PickFileResult> GetFileSavePicker(List<string> extention,string saveName)
    {
        FileSavePicker picker = new FileSavePicker(ApplicationSetup.App.MainWindow.AppWindow.Id);
        picker.FileTypeChoices.Add("主程序", extention);
        picker.SuggestedFileName = saveName;
        PickFileResult file = await picker.PickSaveFileAsync();
        return file;
    }

    public async Task<PickFolderResult> GetFolderPicker()
    {
        FolderPicker openPicker = new FolderPicker(ApplicationSetup.App.MainWindow.AppWindow.Id);
        return await openPicker.PickSingleFolderAsync();
    }
}
