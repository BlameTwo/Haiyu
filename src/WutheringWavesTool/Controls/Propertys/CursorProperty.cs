
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

    // Using a DependencyProperty as the backing store for CursorCode.  This enables animation, styling, binding, etc...
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
            InputCursor customCursor = InputDesktopResourceCursor.CreateFromModule(@"WutheringWavesTool.exe", 111);
            CursorHelper.SetCursor(element, customCursor);
        }
    }
}
