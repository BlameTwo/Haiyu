namespace Haiyu.Converter;

public partial class IntToSecondTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int v)
        {
            return TimeSpan.FromSeconds(v).ToString("hh\\:mm\\:ss");
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
