using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FluentWPF.Converters;

public class AngleConverter: IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double value = (double)values[0];
        double maximum = (double)values[1];

        double angle = (value / maximum) * 360;
        return new RectangleGeometry(new Rect(0, 0, 100, 100), angle, 0.0, new MatrixTransform(new Matrix(angle, 0.0, 0.0, angle, 0.0, 0.0)));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
