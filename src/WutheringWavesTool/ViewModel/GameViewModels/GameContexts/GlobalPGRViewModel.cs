using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel.GameViewModels
{
    public class GlobalPGRViewModel : GameContextViewModelBase
    {
        public GlobalPGRViewModel(
            [FromKeyedServices(nameof(GlobalPRGGameContext))] IGameContext gameContext,
            [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
            IAppContext<App> appContext,
            ITipShow tipShow
        )
            : base(gameContext, dialogManager, appContext, tipShow) { }

        public override void DisposeAfter() { }

        public override Task LoadAfter()
        {
            return Task.CompletedTask;
        }
    }
}
