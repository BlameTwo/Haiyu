using CommunityToolkit.WinUI.Behaviors;
using Microsoft.UI.Input;
using System.Threading.Tasks;
using Waves.Core.Common;
using WutheringWavesTool.Common;
using WutheringWavesTool.Helpers;
using WutheringWavesTool.Pages.GamePages;

namespace WutheringWavesTool.Pages;

public sealed partial class ShellPage : Page
{
    public ShellPage()
    {
        this.InitializeComponent();
        this.ViewModel =
            Instance.GetService<ShellViewModel>() ?? throw new ArgumentException("服务注册错误");
        this.Loaded += ShellPage_Loaded;
        this.ViewModel.HomeNavigationService.Navigated += HomeNavigationService_Navigated;
        this.ViewModel.HomeNavigationService.RegisterView(this.frame);
        this.ViewModel.HomeNavigationViewService.Register(this.navigationView);
        this.ViewModel.TipShow.Owner = this.panel;
        this.ViewModel.Image = this.image;
        this.ViewModel.AppContext.SetTitleControl(this.titlebar);
    }

    private void HomeNavigationService_Navigated(object sender, NavigationEventArgs e)
    {
        if (
            e.SourcePageType == typeof(MainGamePage)
            || e.SourcePageType == typeof(BiliBiliGamePage)
            || e.SourcePageType == typeof(GlobalGamePage)
            || e.SourcePageType == typeof(MainPGRGamePage)
        )
        {
            To0.Start();
            this.titlebar.UpDate();
        }
        else if (e.SourcePageType == typeof(CommunityPage))
        {
            To8.Start();
            this.ViewModel.LoginBthVisibility = Visibility.Collapsed;
            this.titlebar.UpDate();
        }
        else
        {
            To8.Start();
            this.titlebar.UpDate();
        }
        ViewModel.SetSelectItem(e.SourcePageType);
        this.ViewModel.HomeNavigationService.ClearHistory();
        GC.Collect();
    }

    public ShellViewModel ViewModel { get; }

    private async void ShellPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        this.ViewModel.DialogManager.RegisterRoot(this.XamlRoot);
        await this.ViewModel.AppContext.WallpaperService.RegisterImageHostAsync(this.image);
    }

    private void ComboBox_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        this.titlebar.UpDate();
    }
}
