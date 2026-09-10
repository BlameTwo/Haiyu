using Haiyu.Common.Contracts;

namespace Haiyu.Pages;

public sealed partial class WavesAnalysisRecordPage : Page
{

    public WindowSession WindowSession { get; }
    public WavesAnalysisRecordViewModel? ViewModel { get; private set; }

    public WavesAnalysisRecordPage(WindowSession windowSession,WavesAnalysisRecordViewModel viewModel)
    {
        InitializeComponent();
        WindowSession = windowSession;
        this.ViewModel = viewModel;
        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
        SetWindow();
    }

    public void SetWindow()
    {
        this.ViewModel?.Initialization(this.WindowSession.Context.GetWindow());
        this.titleBar.Window = this.WindowSession.Context.GetWindow();
        this.ViewModel.SetSessionAsync(this.WindowSession.GetParameter<CloudGameLoginSession>());
    }

}
