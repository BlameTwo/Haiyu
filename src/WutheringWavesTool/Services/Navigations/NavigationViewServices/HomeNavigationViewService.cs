namespace Haiyu.Services.Navigations.NavigationViewServices;

public class HomeNavigationViewService : NavigationViewServiceBase
{
    public HomeNavigationViewService(
        [FromKeyedServices(nameof(HomeNavigationService))] INavigationService navigationService,
        IPageService pageService
    )
        : base(navigationService, pageService) { }
}
