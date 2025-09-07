namespace WutheringWavesTool.WindowModels;

public sealed partial class GetGeetWindow : WindowModelBase
{
    public GeetType Type { get; }

    public GetGeetWindow(nint value, GeetType type) :base(value)
    {
        this.InitializeComponent();
        this.titleBar.Window = this;
        Type = type;
        this.webView2.NavigationCompleted += WebView2_NavigationCompleted;
        switch (Type)
        {
            case GeetType.Login:

                this.webView2.Source = new(AppDomain.CurrentDomain.BaseDirectory + "Assets\\geet.html");
                break;
            case GeetType.WebGame:
                this.webView2.Source = new(AppDomain.CurrentDomain.BaseDirectory + "Assets\\geet2.html");
                break;
            default:
                break;
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
                new(args.TryGetWebMessageAsString(),Type)
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
