using Haiyu.Models.Dialogs;

namespace Haiyu.Pages.Dialogs;

public sealed partial class UpdateGameDialog : ContentDialog,
            IResultDialog<UpdateGameResult>
{
    public UpdateGameDialog()
    {
        InitializeComponent();
        this.ViewModel = Instance.Host.Services.GetRequiredService<UpdateGameViewModel>();
        this.RequestedTheme = Instance.Host.Services.GetRequiredService<IThemeService>().CurrentTheme;
    }

    public UpdateGameViewModel ViewModel { get; }

    public UpdateGameResult? GetResult()
    {
        return ViewModel.GameResult();
    }

    public void SetData(object data)
    {
        if(data is string str && Instance.Host.Services.GetRequiredKeyedService<IGameContext>(str) is IGameContext context)
        {
            this.ViewModel.SetData(context);
        }
    }
}
