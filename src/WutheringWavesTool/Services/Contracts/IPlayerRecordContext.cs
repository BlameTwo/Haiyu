namespace Haiyu.Services.Contracts;

public interface IPlayerRecordContext : IDisposable
{
    public IDialogManager DialogManager { get; }

    public ITipShow TipShow { get; }

    public IRecordCacheService RecordCacheService { get; }
    public IServiceScope Scope { get; }
    public INavigationService NavigationService { get; }

    public void SetScope(IServiceScope scope);

    public FiveGroupModel FiveGroupModel { get; set; }

    public List<CommunityRoleData> CommunityRoleDatas { get; set; }

    public Task<CreateRecordArgs?> ShowInputRecordAsync(object data);
}
