using FluentWPF.Resources;
using System.Globalization;
using System.Windows.Data;

namespace FluentWPF.Converters;

public class FontIconConverter : IValueConverter
{
    public static FontIconConverter Instance => new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is FontIconEnum font)
        {
            FontIconResource tmp_res = new();
            System.Reflection.PropertyInfo[] propertys = tmp_res.GetType().GetProperties();
            string name = font.ToString();
            foreach (var item in propertys)
            {
                var result = item.Name;
                if (result != name) continue;
                return item.GetValue(tmp_res)!.ToString()!;
            }
        }
        return "NULL";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
