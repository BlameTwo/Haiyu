using Microsoft.UI.Input;

namespace WutheringWavesTool.Helpers;

public class CursorHelper
{
    /// <summary>
    /// 设置控件鼠标指针
    /// </summary>
    /// <param name="uie"></param>
    /// <param name="crusor"></param>
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_ProtectedCursor")]
    public static extern void SetCursor(UIElement uie, InputCursor crusor);


    /// <summary>
    /// 获取控件鼠标指针
    /// </summary>
    /// <param name="uie"></param>
    /// <returns></returns>
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_ProtectedCursor")]
    public static extern InputCursor GetCursor(UIElement uie);
}
