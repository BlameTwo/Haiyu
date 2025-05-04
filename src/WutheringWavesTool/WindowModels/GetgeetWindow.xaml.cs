namespace WutheringWavesTool.WindowModels;

public sealed partial class GetGeetWindow : WindowModelBase
{
    public GetGeetWindow()
    {
        this.InitializeComponent();
        this.titleBar.Window = this;
        this.webView2.NavigationCompleted += WebView2_NavigationCompleted;
        this.webView2.Source = new(AppDomain.CurrentDomain.BaseDirectory + "Assets\\geet.html");
        if (this.Content is FrameworkElement fe)
        {
            fe.RequestedTheme = ElementTheme.Dark;
        }
    }

    private void WebView2_NavigationCompleted(
        WebView2 sender,
        Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args
    )
    {
        sender.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        sender.CoreWebView2.Profile.PreferredColorScheme = CoreWebView2PreferredColorScheme.Dark;
    }

    private void CoreWebView2_WebMessageReceived(
        Microsoft.Web.WebView2.Core.CoreWebView2 sender,
        Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args
    )
    {
        try
        {
            WeakReferenceMessenger.Default.Send<GeeSuccessMessanger>(
                new(args.TryGetWebMessageAsString())
            );
            this.webView2.Close();
            this.Close();
        }
        catch (Exception)
        {
            return;
        }
    }
}
