using Haiyu.Common.Contracts;
using Haiyu.Models.Dialogs;

namespace Haiyu.Pages.Dialogs
{
    public sealed partial class QRLoginDialog : ContentDialog, IDialog
    {
        public QRLoginDialog(
        QrLoginViewModel viewModel,
        IThemeService themeService
    )
    {
        InitializeComponent();
        ViewModel = viewModel;
        RequestedTheme = themeService.CurrentTheme;
    }

        public QrLoginViewModel? ViewModel { get; }

        public void SetData(object data)
        {
        }
    }
}
