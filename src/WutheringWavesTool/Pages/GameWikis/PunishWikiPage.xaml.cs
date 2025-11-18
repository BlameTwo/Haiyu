using Haiyu.ViewModel.WikiViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Haiyu.Pages.GameWikis
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PunishWikiPage : Page, IPage
    {
        public PunishWikiPage()
        {
            InitializeComponent();
            this.ViewModel = Instance.GetService<PunishWikiViewModel>();
        }
        public PunishWikiViewModel ViewModel { get; private set; }
        public Type PageType => typeof(PunishWikiPage);
    }
}
