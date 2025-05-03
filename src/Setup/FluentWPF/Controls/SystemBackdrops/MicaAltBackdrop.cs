using FluentWPF.Common;

namespace FluentWPF.Controls.SystemBackdrops;

/// <summary>
/// 不想多谢判断
/// 在WinUI3得backdrop中是有一个Kind枚举在Mica控制器中，在这里我不加
/// </summary>
public class MicaAltBackdrop : SystemBackdropBase
{
    public MicaAltBackdrop()
        : base(Models.Enums.SystemBackdropTypeEnum.MicaAlt) { }
}
