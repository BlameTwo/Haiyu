namespace Haiyu.Converter;

public partial class StarLevelColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int level)
        {
            if (level == 5)
            {
                return new SolidColorBrush(
                    new Color()
                    {
                        R = 243,
                        G = 255,
                        B = 0,
                        A = 255,
                    }
                );
            }
            if (level == 4)
            {
                return new SolidColorBrush(
                    new Color()
                    {
                        R = 158,
                        G = 0,
                        B = 216,
                        A = 255,
                    }
                );
            }
            if (level == 3)
            {
                return new SolidColorBrush(
                    new Color()
                    {
                        R = 84,
                        G = 237,
                        B = 255,
                        A = 255,
                    }
                );
            }
            if (level == 2)
            {
                return new SolidColorBrush(
                    new Color()
                    {
                        R = 112,
                        G = 209,
                        B = 61,
                        A = 255,
                    }
                );
            }
            if (level == 1)
            {
                return new SolidColorBrush(
                    new Color()
                    {
                        R = 255,
                        G = 255,
                        B = 255,
                        A = 255,
                    }
                );
            }
        }
        return new SolidColorBrush(Colors.White);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
