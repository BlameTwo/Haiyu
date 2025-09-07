namespace WutheringWavesTool.Services.Contracts;

public interface IViewFactorys
{
    public IAppContext<App> AppContext { get; }
    public GetGeetWindow CreateGeetWindow(GeetType type);

    public WindowModelBase ShowSignWindow(GameRoilDataItem role);

    public WindowModelBase ShowRolesDataWindow(ShowRoleData detily);

    public WindowModelBase ShowPlayerRecordWindow();
    public TransparentWindow CreateTransperentWindow();
    public WindowModelBase ShowAdminDevice();
    public WindowModelBase ShowActivityTimeWindow();
    public bool ShowToolWindow();
}
