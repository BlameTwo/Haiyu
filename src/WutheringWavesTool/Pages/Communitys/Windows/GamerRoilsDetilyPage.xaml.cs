using CommunityToolkit.WinUI.Controls;
using Haiyu.Models.Wrapper.WindowRoils;

namespace Haiyu.Pages.Communitys.Windows;

public sealed partial class GamerRoilsDetilyPage : Page, IWindowPage
{
    public GamerRoilsDetilyPage()
    {
        this.InitializeComponent();
        ViewModel = Instance.GetService<GamerRoilsDetilyViewModel>();
        this.title_bth.Click += Button_Click;
        this.Loaded += this.Page_Loaded;
    }

    public GamerRoilsDetilyViewModel ViewModel { get; }

    public void Dispose()
    {
        this.ViewModel.Dispose();
    }

    public void SetData(object value)
    {
        if (value is ShowRoleData data)
        {
            this.ViewModel.Data = data;
            this.ViewModel.SelectCache = data.Id;
        }
    }

    public void SetWindow(Window window)
    {
        this.titlebar.Window = window;
        window.AppWindow.Closing += AppWindow_Closing;
        titlebar.UpDate();
    }

    private void AppWindow_Closing(AppWindow sender, AppWindowClosingEventArgs args)
    {
        this.title_bth.Click -= Button_Click;
        this.Loaded -= this.Page_Loaded;
        this.ViewModel.Dispose();
        GC.Collect();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.view.IsPaneOpen = !this.view.IsPaneOpen;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        titlebar.UpDate();
    }

    private async void view_SelectionChanged(
        NavigationView sender,
        NavigationViewSelectionChangedEventArgs args
    )
    {
        if (args.SelectedItem != null && args.SelectedItem is NavigationRoilsDetilyItem item)
        {
            await this.ViewModel.SwitchPage(item);
        }
    }


    private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems == null || e.AddedItems.Count == 0) return;
        if (this.ViewModel == null)
            return;
        this.ViewModel.GamerRoilViewModel.SetPage((e.AddedItems[0] as SegmentedItem).Tag.ToString());
    }
}
