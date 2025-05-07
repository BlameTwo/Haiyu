namespace WutheringWavesTool.Services.Contracts;

public interface IPickersService
{
    public Task<StorageFolder> GetFolderPicker();

    public Task<StorageFile> GetFileOpenPicker(List<string> extention);

    public Task<StorageFile> GetFileSavePicker();
}
