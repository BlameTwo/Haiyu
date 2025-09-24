
namespace Haiyu.Pages;

public sealed partial class ColorFullGame : Page,IWindowPage
{
    public ColorFullGame()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<ColorFullViewModel>();
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
}
