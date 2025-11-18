using Haiyu.Models.Dialogs;

namespace Haiyu.Pages.Dialogs
{
    public sealed partial class GameLauncherCacheManager : ContentDialog, IDialog
    {
        public GameLauncherCacheManager()
        {
            InitializeComponent();
            this.ViewModel = Instance.GetService<GameLauncherCacheViewModel>();
        }

        public GameLauncherCacheViewModel ViewModel { get; }

        public void SetData(object data)
        {
            if (data is GameLauncherCacheArgs args)
            {
                ViewModel.SetData(args);
            }
        }
    }
}
