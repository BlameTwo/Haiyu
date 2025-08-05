using WutheringWavesTool.Models.Dialogs;

namespace WutheringWavesTool.Pages.Dialogs
{
    public sealed partial class QRLoginDialog : ContentDialog, IResultDialog<QRScanResult>
    {
        public QRLoginDialog()
        {
            InitializeComponent();
            this.ViewModel = Instance.GetService<QrLoginViewModel>();
        }

        public QrLoginViewModel? ViewModel { get; }

        public QRScanResult? GetResult()
        {
            return ViewModel?.Result;
        }

        public void SetData(object data)
        {
        }
    }
}
