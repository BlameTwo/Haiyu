using WutheringWavesTool.Common;
using WutheringWavesTool.Models.Dialogs;

namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class SelectDownloadGameDialog
    : ContentDialog,
        IResultDialog<SelectDownloadFolderResult>
{
    public SelectDownloadGameDialog()
    {
        this.InitializeComponent();
        this.RequestedTheme =
            AppSettings.AppTheme == null ? ElementTheme.Default
            : AppSettings.AppTheme == "Dark" ? ElementTheme.Dark
            : AppSettings.AppTheme == "Light" ? ElementTheme.Light
            : ElementTheme.Default;
        this.ViewModel = Instance.Service.GetRequiredService<SelectDownloadGameViewModel>();
    }

    public SelectDownloadGameViewModel ViewModel { get; }

    public SelectDownloadFolderResult GetResult()
    {
        return new()
        {
            InstallFolder = ViewModel.FolderPath,
            Launcher = ViewModel.Launcher,
            Result = ViewModel.Result,
        };
    }

    public void SetData(object data)
    {
        if (data is Type type)
        {
            ViewModel.SetData(type);
        }
    }
}
