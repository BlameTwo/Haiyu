namespace Haiyu.Services;

public class GamerRoilContext : IGamerRoilContext
{
    public IServiceScope Scope { get; private set; }

    public INavigationService NavigationService { get; private set; }

    public void SetScope(IServiceScope scope)
    {
        this.Scope = scope;
        this.NavigationService = Scope.ServiceProvider.GetRequiredKeyedService<INavigationService>(
            nameof(GameRoilNavigationService)
        );
    }
}
