using Haiyu.Common.Contracts;
using Haiyu.Models.Dialogs;
using Waves.Settings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Haiyu.Pages.Dialogs
{
    public sealed partial class ClearMemoryDialog : ContentDialog, IDialog
    {
        public ClearMemoryDialog(ClearMemoryViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
            this.RequestedTheme = Instance
                .Host.Services.GetRequiredService<IThemeService>()
                .CurrentTheme;
        }

        public ClearMemoryViewModel ViewModel { get; }

        public void SetData(object data) { }
    }
}
