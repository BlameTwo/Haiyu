using Haiyu.Controls.AnimatedTextBlock.Effects;
using Waves.Api.Models.CloudGame;

namespace Haiyu.Pages.Record;

public sealed partial class AnalysisRecordPage : Page, IWindowPage
{
    public AnalysisRecordPage()
    {
        InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<AnalysisRecordViewModel>();
    }

    public AnalysisRecordViewModel ViewModel { get; }

    public void Dispose()
    {
    }

    public void SetData(object value)
    {
        if (value is LoginData data)
        {

            this.ViewModel.LoginData = data;
        }
    }

    public void SetWindow(Window window)
    {
        this.titlebar.Window = window;
    }

    internal void SetData(LoginData data)
    {
    }
}
