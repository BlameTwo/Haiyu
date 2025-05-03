namespace FluentWPF.Models.Args;

public class NavigationItemInvokedArgs:RoutedEventArgs
{
    public NavigationItemInvokedArgs(RoutedEvent @event,object source,NavigationViewItem selectItem)
        :base(@event,source)
    {
        SelectItem = selectItem;
    }

    public NavigationViewItem SelectItem { get; }
}
