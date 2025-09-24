namespace Haiyu.Converter;

public partial class RecordColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int i)
        {
            return new SolidColorBrush(GetColorForScore(i));
        }
        if (value is double d)
        {
            return new SolidColorBrush(GetColorForScore(d));
        }
        return new SolidColorBrush(Colors.Red);
    }

    Color GetColorForScore(double score)
    {
        if (score >= 0 && score < 13.3)
            return Color.FromArgb(255, 0, 128, 0);
        else if (score >= 13.3 && score < 26.7)
            return Color.FromArgb(255, 0, 255, 0);
        else if (score >= 26.7 && score < 40)
            return Color.FromArgb(255, 255, 255, 0);
        else if (score >= 40 && score < 53.3)
            return Color.FromArgb(255, 255, 215, 0);
        else if (score >= 53.3 && score < 66.7)
            return Color.FromArgb(255, 255, 160, 122);
        else if (score >= 66.7 && score <= 80)
            return Color.FromArgb(255, 255, 0, 0);
        else
            return Color.FromArgb(255, 255, 255, 255);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
