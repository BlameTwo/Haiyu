
namespace Haiyu.Pages;

public sealed partial class ColorFullGame : Page,IWindowPage
{
    public ColorFullGame()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<ColorFullViewModel>();
        this.ViewModel.TipShow.Owner = this.grid;
        this.Loaded += ColorFullGame_Loaded;
    }

    private void ColorFullGame_Loaded(object sender, RoutedEventArgs e)
    {
        this.ViewModel.DialogManager.RegisterRoot(this.XamlRoot);
    }

    public ColorFullViewModel ViewModel { get; }

    public void Dispose()
    {
        this.Loaded -= ColorFullGame_Loaded;
    }

    public void SetData(object value)
    {
        throw new NotImplementedException();
    }

    public void SetWindow(Window window)
    {
        this.title.Window = window;
        this.title.Window.Closed += Window_Closed;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        this.Dispose();
    }

    private async void ItemsView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
    {
        await ViewModel.CellClicked(args);
    }
}
