
using Haiyu.Helpers;
using Microsoft.UI.Input;

namespace Haiyu.Controls.Propertys;

public partial class CursorProperty
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
        DependencyProperty.RegisterAttached("CursorCode", typeof(int), typeof(CursorProperty), new PropertyMetadata(-1, OnCursorValueChanged));

    private static void OnCursorValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element && e.NewValue is int value)
        {
            if (value == -1)
            {
                return;
            }
            InputCursor customCursor = InputDesktopResourceCursor.CreateFromModule(@"Haiyu.exe", (uint)value);

        }
    }



    public static CursorType GetCursorType(DependencyObject obj)
    {
        return (CursorType)obj.GetValue(CursorTypeProperty);
    }

    public static void SetCursorType(DependencyObject obj, CursorType value)
    {
        obj.SetValue(CursorTypeProperty, value);
    }

    public static readonly DependencyProperty CursorTypeProperty =
        DependencyProperty.RegisterAttached("CursorType", typeof(CursorType), typeof(CursorProperty), new PropertyMetadata(null));



    public static CursorName GetCursorName(DependencyObject obj)
    {
        return (CursorName)obj.GetValue(CursorNameProperty);
    }

    public static void SetCursorName(DependencyObject obj, CursorName value)
    {
        obj.SetValue(CursorNameProperty, value);
    }

    public static readonly DependencyProperty CursorNameProperty =
        DependencyProperty.RegisterAttached("CursorName", typeof(CursorName), typeof(CursorProperty), new PropertyMetadata(null, OnCursorNameChanged));

    private static void OnCursorNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            var name = CursorProperty.GetCursorName(element);
            var type = CursorProperty.GetCursorType(element);
            CursorHelper.SetCursor(name, type, element);
        }
    }
}
