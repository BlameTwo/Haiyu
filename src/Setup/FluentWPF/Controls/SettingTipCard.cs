using FluentWPF.Common.Bases;

namespace FluentWPF.Controls;

public class SettingTipCard : ControlBase
{
    //创建一个枚举类型来区分提示框类型


    /// <summary>
    /// 提示类型
    /// </summary>
    public SettingTipType TipType
    {
        get { return (SettingTipType)GetValue(TipTypeProperty); }
        set { SetValue(TipTypeProperty, value); }
    }

    public static readonly DependencyProperty TipTypeProperty = DependencyProperty.Register(
        "TipType",
        typeof(SettingTipType),
        typeof(SettingTipCard),
        new PropertyMetadata(SettingTipType.Default)
    );

    /// <summary>
    /// 标题
    /// </summary>
    public object Header
    {
        get { return (object)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        "Header",
        typeof(object),
        typeof(SettingTipCard),
        new PropertyMetadata("")
    );

    /// <summary>
    /// 说明
    /// </summary>
    public object Description
    {
        get { return (object)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        "Description",
        typeof(object),
        typeof(SettingTipCard),
        new PropertyMetadata(null)
    );
}
