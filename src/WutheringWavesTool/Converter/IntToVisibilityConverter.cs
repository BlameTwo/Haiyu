using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Haiyu.Converter;

public class IntToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int count && parameter is string targetCount)
        {

            if (int.TryParse(targetCount, out int target) && count == target)
            {
                return Visibility.Visible;
            }
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}