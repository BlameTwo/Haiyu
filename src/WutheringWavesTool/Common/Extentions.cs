using System.Collections.ObjectModel;
using Windows.Graphics;

namespace Haiyu.Common;

public static class Extentions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> ts)
    {
        if (ts == null)
            return new ObservableCollection<T>();
        ObservableCollection<T> result = new();
        foreach (var t in ts)
        {
            result.Add(t);
        }
        return result;
    }

    public static CardItemObservableCollection<T> ToCardItemObservableCollection<T>(
        this IEnumerable<T> ts
    )
    {
        return new CardItemObservableCollection<T>(ts);
    }

    public static void RemoveAll<T>(this ObservableCollection<T> ts)
    {
        if (ts == null || ts.Count == 0)
            return;
        foreach (var item in ts.ToList())
        {
            ts.Remove(item);
        }
    }

    public static RectInt32? GetControlRect(
        this FrameworkElement control,
        Controls.TitleBar titleBar
    )
    {
        if (titleBar.Title is FrameworkElement header)
        {
            if (header != null)
            {
                var ScaleAdjustment = Controls.TitleBar.GetScaleAdjustment(titleBar.Window);
                var value = new RectInt32();
                value.X = (int)((control.ActualWidth + header.ActualWidth) * ScaleAdjustment);
                value.Y = 0;
                value.Height = (int)(control.ActualHeight * ScaleAdjustment);
                value.Width = (int)(control.ActualWidth * ScaleAdjustment);
                return value;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
