using Haiyu.ViewModel.GameViewModels;

namespace Haiyu.Pages.GamePages
{
    public sealed partial class MainPGRGamePage : Page, IPage
    {
        public MainPGRGamePage()
        {
            InitializeComponent();
            this.ViewModel = Instance.GetService<MainPGRViewModel>();
        }

        private void SelectorBar_SelectionChanged(
            SelectorBar sender,
            SelectorBarSelectionChangedEventArgs args
        )
        {
            this.ViewModel.SelectTab(sender.SelectedItem.Text);
        }

        public MainPGRViewModel ViewModel { get; }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ViewModel.Dispose();
            this.Bindings.StopTracking();
        }

        public Type PageType => typeof(MainPGRGamePage);
    }
}
