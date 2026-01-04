namespace Haiyu.Converter;

public partial class RecordColorItemConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int intValue)
        {
            if (intValue == 4)
            {
                return new SolidColorBrush(
                   new Color()
                   {
                       R = 151,
                       G = 64,
                       B = 251,
                       A = 255,
                   }
               );
            }
            else if (intValue == 5)
            {
                return new SolidColorBrush(
                    new Color()
                    {
                        R = 255,
                        G = 118,
                        B = 49,
                        A = 255,
                    }
                );
            }
        }
        return new SolidColorBrush(
                    new Color()
                    {
                        R = 68,
                        G = 161,
                        B = 164,
                        A = 255,
                    }
                );
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
