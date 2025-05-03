using FluentWPF.Common.Bases;
using FluentWPF.Contracts.Navigations;

namespace FluentWPF.Controls;

public class NavigationViewHeaderItem : ControlBase, INavigationViewItem
{

    public object Content
    {
        get { return (object)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(object), typeof(NavigationViewHeaderItem), new PropertyMetadata(""));

    public string Header
    {
        get { return (string)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public  static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(string), typeof(NavigationViewHeaderItem), new PropertyMetadata(null));

    public void RefreshPanel(NavigationDisplayMode mode)
    {
        if(mode == NavigationDisplayMode.Close)
        {
            this.Visibility = Visibility.Collapsed;
            return;
        }
        this.Visibility = Visibility.Visible;
    }

    public void RefreshPanel(NavigationDisplayMode mode, bool isOpen)
    {
        if(isOpen)
            this.Visibility= Visibility.Visible;
        else 
            this.Visibility= Visibility.Collapsed;
    }
}
