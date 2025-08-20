
using Microsoft.UI.Input;
using WutheringWavesTool.Helpers;

namespace WutheringWavesTool.Controls.Propertys;

public class CursorProperty
{

    public static int GetCursorCode(DependencyObject obj)
    {
        return (int)obj.GetValue(CursorCodeProperty);
    }

    public static void SetCursorCode(DependencyObject obj, int value)
    {
        obj.SetValue(CursorCodeProperty, value);
    }

    public static readonly DependencyProperty CursorCodeProperty =
        DependencyProperty.RegisterAttached("CursorCode", typeof(int), typeof(CursorProperty), new PropertyMetadata(-1,OnCursorValueChanged));

    private static void OnCursorValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if(d is UIElement element && e.NewValue is int value)
        {
            if(value == -1)
            {
                return;
            }
            InputCursor customCursor = InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", (uint)value);
            CursorHelper.SetCursor(element, customCursor);
        }
    }
}
