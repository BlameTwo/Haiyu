namespace Haiyu.Pickers;

public interface IPickersService
{
    public void InitWindow(nint handle);

    Task<PickFileResult?> GetFileOpenPicker(IReadOnlyCollection<string> extensions);

    Task<PickFileResult?> GetFileSavePicker(IReadOnlyCollection<string> extensions, string saveName);

    Task<PickFolderResult?> GetFolderPicker();
}
