using Windows.Graphics;
using TitleBar = Haiyu.Controls.TitleBar;

namespace Haiyu;

public sealed partial class ToolWindow : WindowEx
{
    public ToolWindow()
    {
        InitializeComponent();
        this.SystemBackdrop = new DesktopAcrylicBackdrop();
        AppWindow.IsShownInSwitchers = false;
        AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        this.MaxHeight = 200;
        this.MaxWidth = 300;
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsResizable = true;
            presenter.SetBorderAndTitleBar(true, false);
        }
        this.SizeChanged += ToolWindow_SizeChanged;
    }

    private void ToolWindow_SizeChanged(object sender, WindowSizeChangedEventArgs args)
    {
        this.AppWindow.TitleBar.SetDragRectangles(
            [
                new RectInt32()
                {
                    Height = 36,
                    Width = this.AppWindow.Size.Width,
                    X = 0,
                    Y = 0,
                },
            ]
        );
    }

    private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        this.AppWindow.TitleBar.SetDragRectangles(
            [
                new RectInt32()
                {
                    Height = 36,
                    Width = (int)e.NewSize.Width,
                    X = 0,
                    Y = 0,
                },
            ]
        );
    }

    private void grid_PointerEntered(
        object sender,
        Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e
    )
    {
        grid.Visibility = Visibility.Visible;
    }

    private void grid_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        grid.Visibility = Visibility.Collapsed;
    }

    private void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton button)
        {
            if (button.IsChecked == true)
            {
                this.AppWindow.TitleBar.SetDragRectangles([new RectInt32(0, 0, 0, 0)]);
            }
            else
            {
                var ScaleAdjustment = TitleBar.GetScaleAdjustment(this);
                this.AppWindow.TitleBar.SetDragRectangles(
                    [
                        new RectInt32()
                        {
                            Height = 36,
                            Width = (int)(grid.ActualWidth * ScaleAdjustment),
                            X = 0,
                            Y = 0,
                        },
                    ]
                );
            }
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.SizeChanged -= ToolWindow_SizeChanged;
        this.Close();
    }
}
