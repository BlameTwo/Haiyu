using FluentWPF.Contracts.Navigations;
using FluentWPF.Models;

namespace FluentWPF;
/// <summary>
/// 路由事件的基础
/// </summary>
/// <typeparam name="Sender"></typeparam>
/// <typeparam name="Arg"></typeparam>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void ControlEventHandler<in Sender,in Arg>(Sender sender,Arg args)
    where Sender:DependencyObject
    where Arg:RoutedEventArgs;
