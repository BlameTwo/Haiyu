namespace Haiyu.Converter;

public partial class RecordStarConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
        {
            return "";
        }
        if (value is bool item)
        {
            if (!item)
                return "中";
            else
            {
                return "歪";
            }
        }
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
