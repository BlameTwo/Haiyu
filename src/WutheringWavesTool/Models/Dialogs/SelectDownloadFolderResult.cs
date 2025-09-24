namespace Haiyu.Models.Dialogs;

public class SelectDownloadFolderResult
{
    public ContentDialogResult? Result { get; internal set; }

    public string InstallFolder { get; set; }
    public GameLauncherSource? Launcher { get; internal set; }
}
