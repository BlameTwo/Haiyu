namespace FluentWPF.Models.Args
{
    public class NavigationSelectionChangedArgs : RoutedEventArgs
    {
        public NavigationSelectionChangedArgs(RoutedEvent @event, object source, NavigationViewItem item)
            : base(@event, source)
        {
            SelectItem = item;
        }
        public NavigationViewItem SelectItem { get; set; }
    }
}
