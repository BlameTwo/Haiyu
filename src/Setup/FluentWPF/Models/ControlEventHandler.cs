using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentWPF.Models;

public delegate void TypedEventHandler<in TSource, in TArgs>(TSource sender, TArgs args)
    where TSource : DependencyObject
    where TArgs : RoutedEventArgs;

