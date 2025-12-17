namespace Haiyu.ViewModel;

public sealed partial class ToolViewModel : ViewModelBase
{
    public ToolViewModel(
        [FromKeyedServices(nameof(HomeNavigationService))] INavigationService navigationService
    )
    {
        NavigationService = navigationService;
    }

    [ObservableProperty]
    public partial List<ToolItem> Items { get; set; } = ToolItem.GetDefault();
    public INavigationService NavigationService { get; }

    [RelayCommand]
    void NavigationTo(ItemsViewItemInvokedEventArgs args) 
    {
        if (args.InvokedItem == null || args.InvokedItem is not ToolItem item)
            return;
        NavigationService.NavigationTo(item.Key,null,new EntranceNavigationTransitionInfo());
    }
}

public partial class ToolItem : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial string Key { get; set; }

    [ObservableProperty]
    public partial string Icon { get; private set; }

    public static List<ToolItem> GetDefault() =>
        [
            new ToolItem()
            {
                Key = "Haiyu.ViewModel.ToolViewModels.GameServreViewModel",
                Name = "Switch Server",
                Icon = "\U0001F47B",
            },
        ];
}
