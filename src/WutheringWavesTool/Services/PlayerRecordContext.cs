namespace Haiyu.Services;

public class PlayerRecordContext : Contracts.IPlayerRecordContext
{
    private bool disposedValue;

    public IDialogManager DialogManager { get; }

    public IServiceScope Scope { get; private set; }

    public ITipShow TipShow { get; private set; }
    public INavigationService NavigationService { get; }

    public IRecordCacheService RecordCacheService { get; }

    public FiveGroupModel FiveGroupModel { get; set; }

    public List<CommunityRoleData> CommunityRoleDatas { get; set; }

    public PlayerRecordContext(
        IDialogManager dialogManager,
        ITipShow tipShow,
        [FromKeyedServices(nameof(RecordNavigationService))] INavigationService navigationService,
        IRecordCacheService recordCacheService
    )
    {
        DialogManager = dialogManager;
        TipShow = tipShow;
        NavigationService = navigationService;
        RecordCacheService = recordCacheService;
    }

    public void SetScope(IServiceScope scope)
    {
        this.Scope = scope;
    }

    public async Task<CreateRecordArgs?> ShowInputRecordAsync(object data)
    {
        var dialog = new InputRecordCardDialog();
        dialog.ViewModel = new InputRecordCardViewModel(Scope);
        dialog.SetData(data);
        dialog.XamlRoot = DialogManager.Root;
        DialogManager.SetDialog(dialog);
        await dialog.ShowAsync();
        return dialog.ViewModel.Args;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.NavigationService.UnRegisterView();
                this.TipShow.Owner = null;
                this.Scope.Dispose();
                this.DialogManager.SetDialog(null);
            }

            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~PlayerRecordContext()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
