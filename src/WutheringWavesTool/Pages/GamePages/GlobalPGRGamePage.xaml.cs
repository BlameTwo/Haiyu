// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using WutheringWavesTool.ViewModel.GameViewModels;

namespace WutheringWavesTool.Pages.GamePages
{
    public sealed partial class GlobalPGRGamePage : Page,IPage
    {
        public GlobalPGRGamePage()
        {
            this.InitializeComponent();
            this.ViewModel = Instance.Service.GetRequiredService<GlobalPGRViewModel>();
        }

        public Type PageType => typeof(GlobalPGRGamePage);

        public GlobalPGRViewModel ViewModel { get; }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ViewModel.Dispose();
            this.Bindings.StopTracking();
        }
    }
}
