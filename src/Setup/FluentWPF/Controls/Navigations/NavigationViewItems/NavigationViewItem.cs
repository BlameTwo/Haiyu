using FluentWPF.Common.Bases;
using FluentWPF.Contracts.Navigations;
using FluentWPF.Models.Args;
using FluentWPF.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FluentWPF.Controls;

[TemplateVisualState(GroupName ="DefaultMoveGroup",Name ="EnterUp")]
[TemplateVisualState(GroupName ="DefaultMoveGroup",Name ="EnterDown")]
[TemplateVisualState(GroupName ="DefaultMoveGroup",Name ="LeaveUp")]
[TemplateVisualState(GroupName ="DefaultMoveGroup",Name ="LeaveDown")]
[TemplateVisualState(GroupName ="DefaultMoveGroup",Name ="Default")]
public partial class NavigationViewItem : ButtonBase, INavigationViewItem
{
    public NavigationViewItem() { }

    public FontIconEnum Icon
    {
        get { return (FontIconEnum)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        "Icon",
        typeof(FontIconEnum),
        typeof(NavigationViewItem),
        new PropertyMetadata(null)
    );

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
    }

    protected override void OnClick()
    {
        this.RaiseEvent(new NavigationItemInvokedArgs(ItemInvokedEvent, this, this));
        var navigation = NavigationView.GetNavigationView(this);
        navigation?.OnSelected(this);
        base.OnClick();
    }


    public void RefreshPanel(NavigationDisplayMode mode, bool isOpen)
    {

    }

    public bool IsSelect
    {
        get { return (bool)GetValue(IsSelectProperty); }
        set { SetValue(IsSelectProperty, value); }
    }

    public static readonly DependencyProperty IsSelectProperty = DependencyProperty.Register(
        "IsSelect",
        typeof(bool),
        typeof(NavigationViewItem),
        new PropertyMetadata(false)
    );

    public static readonly RoutedEvent ItemInvokedEvent = EventManager.RegisterRoutedEvent(
        "ItemInvokedEvent",
        RoutingStrategy.Bubble,
        typeof(ControlEventHandler<NavigationViewItem, NavigationItemInvokedArgs>),
        typeof(NavigationViewItem)
    );

    public event ControlEventHandler<NavigationViewItem, NavigationItemInvokedArgs> ItemInvoked
    {
        add =>AddHandler(ItemInvokedEvent, value);
        remove => RemoveHandler(ItemInvokedEvent, value);
    }
}

