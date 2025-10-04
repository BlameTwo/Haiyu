
using Haiyu.Models.Enums;
using System.Net.Sockets;

namespace Haiyu.Converter;

public partial class GameFullModeConverter : IValueConverter
{
    public GameMode ParentMode { get; set; }
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is GameMode mode)
        {
            if (mode == ParentMode)
                return true;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if(value is bool b)
        {
            if(b == true)
            {
                return ParentMode;
            }
        }
        return null;
    }
}
