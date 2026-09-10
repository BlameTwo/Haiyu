using System;
using System.Collections.Generic;
using System.Text;

namespace Haiyu.Converter;

public partial class MonitorMemoryVisiblity : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is double d)
        {
            if (d < 1.5)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
