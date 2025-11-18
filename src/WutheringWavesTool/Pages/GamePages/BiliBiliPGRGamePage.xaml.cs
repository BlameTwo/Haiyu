using Haiyu.ViewModel.GameViewModels;

namespace Haiyu.Pages.GamePages
{
    public sealed partial class BiliBiliPGRGamePage : Page, IPage
    {
        public BiliBiliPGRGamePage()
        {
            this.InitializeComponent();
            this.ViewModel = Instance.Service.GetRequiredService<BiliBiliPGRGameViewModel>();
        }

        public Type PageType => typeof(BiliBiliPGRGamePage);

        public BiliBiliPGRGameViewModel ViewModel { get; }



        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ViewModel.Dispose();
            this.Bindings.StopTracking();
        }
    }
}
