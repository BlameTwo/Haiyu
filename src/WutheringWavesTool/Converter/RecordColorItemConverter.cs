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
                        R = 147,
                        G = 0,
                        B = 255,
                        A = 255,
                    }
                );
            }
            else if (intValue == 5)
            {
                return new SolidColorBrush(
                    new Color()
                    {
                        R = 249,
                        G = 255,
                        B = 60,
                        A = 255,
                    }
                );
            }
        }
        return (SolidColorBrush)App.Current.Resources["TextFillColorPrimaryBrush"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
