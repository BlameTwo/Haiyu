namespace WutheringWavesTool.Controls;

partial class TitleBar
{
    internal void UpDate()
    {
        if (this.Window == null)
            return;
        if (!this.IsExtendsContentIntoTitleBar)
        {
            this.Window.AppWindow.TitleBar.ExtendsContentIntoTitleBar = false;
            this.SizeChanged -= TitleBar_SizeChanged;
            return;
        }
        else
        {
            this.Window.AppWindow.TitleBar.PreferredHeightOption = this.TitleMode;
        }
        if (this.TitleMode == Microsoft.UI.Windowing.TitleBarHeightOption.Tall)
        {
            VisualStateManager.GoToState(this, "Tall", true);
        }
        else if (this.TitleMode == Microsoft.UI.Windowing.TitleBarHeightOption.Standard)
        {
            VisualStateManager.GoToState(this, "Standard", true);
        }
        if (Window.Content is FrameworkElement rootElement)
        {
            UpdateTitleBarCaption(rootElement);
            rootElement.ActualThemeChanged += (s, e) =>
            {
                UpdateTitleBarCaption(rootElement);
            };
        }
        MakeDragLength();
    }

    private void UpdateTitleBarCaption(FrameworkElement rootElement)
    {
        Window.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        Window.AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        if (rootElement.ActualTheme == ElementTheme.Dark)
        {
            Window.AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
            Window.AppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
        }
        else
        {
            Window.AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
            Window.AppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
        }
    }

    private static void OnWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TitleBar title)
        {
            title.SetTitleBar();
        }
    }

    private static void OnTitleBarModeChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is TitleBar title && title.Window != null)
            title.UpDate();
    }

    private static void OnMaxButtonVisibilityChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is TitleBar title && title.Window != null && e.NewValue is bool ismax)
        {
            if (title.Window.AppWindow.Presenter is OverlappedPresenter opr)
            {
                opr.IsMaximizable = ismax;
                title.UpDate();
            }
        }
    }

    private static void OnMinButtonVisibilityChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is TitleBar title && title.Window != null && e.NewValue is bool ismin)
        {
            if (title.Window.AppWindow.Presenter is OverlappedPresenter opr)
            {
                opr.IsMinimizable = ismin;
                title.UpDate();
            }
        }
    }

    private static void OnContentRectChange(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is TitleBar title && title.Window != null)
            title.UpDate();
    }
}
