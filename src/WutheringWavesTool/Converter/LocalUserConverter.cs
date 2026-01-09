
namespace Haiyu.Converter;

public sealed partial class LocalUserColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is bool b)
        {
            if(b)
            {
                return Microsoft.UI.Xaml.Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"];
            }
            else
            {
            }
        }
        return Microsoft.UI.Xaml.Application.Current.Resources["CardBackgroundFillColorDefaultBrush"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
