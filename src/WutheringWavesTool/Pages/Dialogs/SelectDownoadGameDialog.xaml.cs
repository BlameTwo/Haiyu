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
            DialogManager = Instance.Service.GetRequiredKeyedService<IDialogManager>(
                nameof(MainDialogService)
            );
            this.Pickers = Instance.GetService<IPickersService>()!;
        }

        public IDialogManager DialogManager { get; }
        public IPickersService Pickers { get; }

        public SelectDownloadFolderResult GetResult()
        {
            return new SelectDownloadFolderResult();
        }

        public void SetData(object data) { }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = await Pickers.GetFolderPicker();
            if (dialog != null)
            {
                MessageBox.Show(dialog.Path);
            }
        }
    }
}
