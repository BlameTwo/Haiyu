using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel.GameViewModels
{
    public class BiliBiliPGRGameViewModel: GameContextViewModelBase
    {
        public BiliBiliPGRGameViewModel(
            [FromKeyedServices(nameof(BiliBiliPRGGameContext))] IGameContext gameContext,
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
