
namespace Haiyu.Pages;

public sealed partial class ColorFullGame : Page,IWindowPage
{
    public ColorFullGame()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<ColorFullViewModel>();
        this.ViewModel.TipShow.Owner = this.grid;
    }

    public ColorFullViewModel ViewModel { get; }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void SetData(object value)
    {
        throw new NotImplementedException();
    }

    public void SetWindow(Window window)
    {
        this.title.Window = window;
    }

    private async void ItemsView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
    {
        await ViewModel.CellClicked(args);
    }
}
