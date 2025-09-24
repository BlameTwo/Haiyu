namespace Haiyu.Converter;

public partial class CardPoolTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value != null && value is CardPoolType type)
        {
            switch (type)
            {
                case CardPoolType.RoleActivity:
                    return "角色活动换取";
                case CardPoolType.WeaponsActivity:
                    return "武器活动换取";
                case CardPoolType.RoleResident:
                    return "角色常驻换取";
                case CardPoolType.WeaponsResident:
                    return "武器常驻换取";
                case CardPoolType.Beginner:
                    return "新手换取";
                case CardPoolType.BeginnerChoice:
                    return "新手自选";
                case CardPoolType.GratitudeOrientation:
                    return "感恩定向";
                default:
                    break;
            }
        }
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
