using Microsoft.UI.Input;
using Windows.UI.Core;

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

    public static void SetCursor(CursorName name, CursorType type, UIElement element)
    {
        switch (name)
        {
            case CursorName.None:
                SetCursor(element,GetDefault(type));
                break;
            case CursorName.ShouAnRen:
                SetCursor(element, GetShouAnRen(type));
                break;
            case CursorName.FuLuoLuo:
                SetCursor(element, GetFuLuoLuo(type));
                break;
            case CursorName.KaTiXiYa:
                SetCursor(element, GetKaTiCode(type));
                break;
            default:
                break;
        }
    }

    public static InputCursor GetKaTiCode(CursorType type)
    {`
        switch (type)
        {
            case CursorType.None:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 108);
            case CursorType.Help:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 106);
            case CursorType.Link:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 107);
            case CursorType.Normal:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 108);
            case CursorType.Text:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 109);
            case CursorType.Working:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 110);
            default:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 108);
        }
    }

    public static InputCursor GetFuLuoLuo(CursorType type)
    {
        switch (type)
        {
            case CursorType.None:

                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 103);
            case CursorType.Help:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 101);
            case CursorType.Link:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 102);
            case CursorType.Normal:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 103);
            case CursorType.Text:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 104);
            case CursorType.Working:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 105);
            default:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 103);
        }
    }

    public static InputCursor GetShouAnRen(CursorType type)
    {
        switch (type)
        {
            case CursorType.None:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 113);
            case CursorType.Help:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 111);
            case CursorType.Link:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 112);
            case CursorType.Normal:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 113);
            case CursorType.Text:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 114);
            case CursorType.Working:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 115);
            default:
                return InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 113);
        }
    }

    public static InputCursor GetDefault(CursorType type)
    {
        switch (type)
        {
            case CursorType.Help:
                return InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Help,1));
            case CursorType.Link:
                return InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Hand, 1));
                
            case CursorType.Normal:
                return InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 1));
            case CursorType.Text:
                return InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.IBeam, 1));
            case CursorType.Working:
                return InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Wait, 1));
        }
        return InputCursor.CreateFromCoreCursor(new CoreCursor(CoreCursorType.Arrow, 1));
    }
}
