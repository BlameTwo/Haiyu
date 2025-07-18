using Microsoft.UI.Xaml;
using WutheringWavesTool.ViewModel.WinViewModel;

namespace WutheringWavesTool.WindowsView;

public sealed partial class ComputerCountWindow : TransparentWindow, IDisposable
{
    public ComputerCountWindow()
    {
        InitializeComponent();
        this.ViewModel = Instance.GetService<ComputerCountViewModel>()!;
        ViewModel.Window = this;
    }

    public ComputerCountViewModel ViewModel { get; set; }

    public void Dispose()
    {
        this.ViewModel.Clear();
    }
}
