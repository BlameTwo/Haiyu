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
            this.Pickers = Instance.GetService<IPickersService>();
        }

        public IPickersService Pickers { get; }

        public SelectDownloadFolderResult GetResult()
        {
            return new SelectDownloadFolderResult();
        }

        public void SetData(object data) { }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var result = await Pickers.GetFolderPicker();
        }
    }
}
