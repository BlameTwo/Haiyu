namespace Haiyu.Common;

public sealed partial class CardItemObservableCollection<T> : ObservableCollection<T>
{
    public CardItemObservableCollection(IEnumerable<T> items)
        : base(items) { }

    private bool _suppressNotification = false;

    public void RemoveAllItem()
    {
        if (Items == null || Items.Count == 0)
            return;
        _suppressNotification = true;

        try
        {
            while (Items.Count > 0)
            {
                Items.RemoveAt(0);
            }
        }
        finally
        {
            _suppressNotification = false;
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
            );
        }
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!_suppressNotification)
        {
            base.OnCollectionChanged(e);
        }
    }

    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!_suppressNotification)
        {
            base.OnPropertyChanged(e);
        }
    }
}
