
namespace Haiyu.Pages
{
    public sealed partial class StartColorFullGamePage : Page, IWindowPage
    {
        public StartColorFullGamePage()
        {
            InitializeComponent();
            this.ViewModel = Instance.GetService<StartColorFullGameViewModel>();
            this.ViewModel.TipShow.Owner = this.grid;
        }

        public Window Window { get; private set; }
        public StartColorFullGameViewModel ViewModel { get; }

        public void Dispose()
        {
        }

        public void SetData(object value)
        {
        }

        public void SetWindow(Window window)
        {
            this.Window = window;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.title.Window = this.Window;
        }
    }
}
