using FluentWPF.Contracts.Navigations;
using FluentWPF.Models.Args;
using System.Runtime.CompilerServices;

namespace FluentWPF.Controls;

partial class NavigationView
{
    private static void OnDisplayModeChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (e.NewValue is NavigationDisplayMode mode && d is NavigationView view)
        {
            switch (mode)
            {
                case NavigationDisplayMode.Open:
                    VisualStateManager.GoToState(view, "Open", false);
                    view.IsPaneOpen = true;
                    break;
                case NavigationDisplayMode.Close:
                    VisualStateManager.GoToState(view, "Close", false);
                    view.IsPaneOpen = false;
                    break;
            }
            view.UpDate();
        }
    }

    internal void UpDate()
    {
        foreach (var item in MenuItems)
        {
            item.RefreshPanel(this.DisplayMode,this.IsPaneOpen);
        }
        foreach (var item in FooterItems)
        {
            item.RefreshPanel(this.DisplayMode,this.IsPaneOpen);
        }
    }

    private static void OnIsPaneOpenChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (e.NewValue is bool val && d is NavigationView view)
        {
            VisualStateManager.GoToState(view, val == true ? "Open" : "Close", false);
            view.UpDate();
        }
    }

    private void NavigationView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (this.DisplayMode != NavigationDisplayMode.Auto)
            return;
        if (this.ActualWidth < 600)
        {
            VisualStateManager.GoToState(this, "DefaultAutoOpen", false);
            VisualStateManager.GoToState(this, "Close", false);
            this.IsPaneOpen = false;
        }
        else
        {
            VisualStateManager.GoToState(this, "MinAutoOpen", false);
            VisualStateManager.GoToState(this, "Open", false);
            this.IsPaneOpen = true;
        }
        this.UpDate();
    }

    /*
     TopStart,
     TopEnd,
     BootomStart,
     BootomEnd
     */

    internal void OnSelected(INavigationViewItem item)
    {
        if (item is not NavigationViewItem newitem)
            return;
        if (item is not FrameworkElement obj)
            return;
        if (this.SelectItem == null || this.SelectItem == item)
        {
            VisualStateManager.GoToState(obj, "Default", false);
            this.SelectItem = newitem;
            newitem.IsSelect = true;
        }
        else if (this.SelectItem != item)
        {
            bool isCt = false;
            int oldindex = -1, newindex = -1;
            //新项目是否在主菜单中
            var isMenu = MenuItems.IndexOf(newitem) != -1;
            //新项目是否在脚部菜单
            var isFolter = FooterItems.IndexOf(newitem) != -1;
            //旧项目是否在主菜单中
            var oldisMenu = MenuItems.IndexOf(this.SelectItem) != -1;
            //旧项目是否在脚部菜单
            var oldisFolter = FooterItems.IndexOf(this.SelectItem) != -1;
            if (isMenu && !oldisMenu) //新项目在主菜单，旧项目不在，从下到上
            {
                VisualStateManager.GoToState((NavigationViewItem)this.SelectItem,"LeaveUp",false);
                VisualStateManager.GoToState((FrameworkElement)item, "EnterDown", false);
            }
            if (isMenu == false && oldisMenu) //新项目在脚步菜单，旧项目在主菜单
            {
                VisualStateManager.GoToState((NavigationViewItem)this.SelectItem, "LeaveDown", false);
                VisualStateManager.GoToState((FrameworkElement)item, "EnterUp", false);
            }
            else if(isMenu && oldisMenu) //新项目和旧项目同时在主菜单
            {
                newindex = MenuItems.IndexOf(item);
                oldindex = MenuItems.IndexOf(this.SelectItem);
                isCt = true;
            }
            else if(isFolter && oldisFolter) //新项目和旧项目同时在脚部菜单
            {
                newindex = FooterItems.IndexOf(item);
                oldindex = FooterItems.IndexOf(this.SelectItem);
                isCt = true;
            }
            if (isCt && (newindex != -1 && oldindex != -1)) // 是否同时在一个集合中,且index都正确
            {
                if (newindex > oldindex)//如果新项目的索引值大于旧项目
                {
                    // 从旧项目的位置移动下来
                    VisualStateManager.GoToState((NavigationViewItem)this.SelectItem, "LeaveDown", false);
                    VisualStateManager.GoToState((FrameworkElement)item, "EnterUp", false);
                }
                else
                {
                    VisualStateManager.GoToState((FrameworkElement)this.SelectItem, "LeaveUp", false);
                    VisualStateManager.GoToState(obj, "EnterDown", true);
                    // 从旧项目的位置移动上去
                }
            }
            (this.SelectItem as NavigationViewItem)!.IsSelect = false;
            this.SelectItem = item;
            newitem.IsSelect = true;
        }
        RaiseEvent(new NavigationSelectionChangedArgs(NavigationSelectionEvent,this,(NavigationViewItem)item));
    }

    public static readonly RoutedEvent NavigationSelectionEvent = EventManager.RegisterRoutedEvent
        (nameof(NavigationSelectionEvent), 
        RoutingStrategy.Bubble,
        typeof(ControlEventHandler<NavigationView, NavigationSelectionChangedArgs>),
        typeof(NavigationView));

    public event ControlEventHandler<NavigationView, NavigationSelectionChangedArgs> NavigationSelection
    {
        add => AddHandler(NavigationSelectionEvent, value);
        remove => RemoveHandler(NavigationSelectionEvent, value);
    }

    private static void OnSelectItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if(e.NewValue != null &&d is  NavigationView view)
        {
            view!.OnSelected((NavigationViewItem)e.NewValue);
        }
    }
}
