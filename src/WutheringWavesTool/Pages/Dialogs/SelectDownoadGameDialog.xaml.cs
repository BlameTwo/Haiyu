using WutheringWavesTool.Models.Dialogs;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.Pages.Dialogs
{
    public sealed partial class SelectDownoadGameDialog
        : ContentDialog,
            IResultDialog<SelectDownloadFolderResult>
    {
        public SelectDownoadGameDialog()
        {
            InitializeComponent();
            this.ViewModel = Instance.Service.GetRequiredService<SelectDownloadGameViewModel>();
        }

        internal SelectDownloadGameViewModel ViewModel { get; }

        public SelectDownloadFolderResult GetResult()
        {
            return new SelectDownloadFolderResult();
        }

        public void SetData(object data) { }
    }
}
