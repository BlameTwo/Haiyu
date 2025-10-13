using Haiyu.ViewModel.WikiViewModels;

namespace Haiyu.Pages.GameWikis;

public sealed partial class WavesWikiPage : Page
{
    public WavesWikiPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<WavesWikiViewModel>();
    }

    public WavesWikiViewModel ViewModel { get; private set; }
}