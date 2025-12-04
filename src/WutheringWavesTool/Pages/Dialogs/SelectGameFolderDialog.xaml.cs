using Haiyu.Models.Dialogs;

namespace Haiyu.Pages.Dialogs
{
    public sealed partial class SelectGameFolderDialog
        : ContentDialog,
            IResultDialog<SelectDownloadFolderResult>
    {
        public SelectGameFolderDialog()
        {
            this.InitializeComponent();
            this.ViewModel = Instance.Service.GetRequiredService<SelectGameFolderViewModel>();
        }

        public SelectGameFolderViewModel ViewModel { get; }

        public SelectDownloadFolderResult GetResult()
        {
            return new()
            {
                Result = ViewModel.Result,
                InstallFolder = System.IO.Path.GetDirectoryName(ViewModel.ExePath),
                Launcher = ViewModel.Launcher,
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
}
