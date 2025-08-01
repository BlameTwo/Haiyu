using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.ViewModel.GameViewModels
{
    public partial class MainPGRViewModel : GameContextViewModelBase
    {
        public MainPGRViewModel(
            [FromKeyedServices(nameof(MainPGRGameContext))] IGameContext gameContext,
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
