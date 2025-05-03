using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FluentWPF.Controls;

public class FluentProgressRing:ProgressBar
{
    public FluentProgressRing()
    {
    }





    public double ArcBorderThickness
    {
        get { return (double)GetValue(ArcBorderThicknessProperty); }
        set { SetValue(ArcBorderThicknessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ArcThickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ArcBorderThicknessProperty =
        DependencyProperty.Register("ArcBorderThickness", typeof(double), typeof(FluentProgressRing), new PropertyMetadata(null));



    public Thickness ArcThickness
    {
        get { return (Thickness)GetValue(ArcThicknessProperty); }
        set { SetValue(ArcThicknessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ArcThickness.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ArcThicknessProperty =
        DependencyProperty.Register("ArcThickness", typeof(Thickness), typeof(FluentProgressRing), new PropertyMetadata(null));





    public SolidColorBrush ArcFillBrush
    {
        get { return (SolidColorBrush)GetValue(ArcFillBrushProperty); }
        set { SetValue(ArcFillBrushProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ArcFillBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ArcFillBrushProperty =
        DependencyProperty.Register("ArcFillBrush", typeof(SolidColorBrush), typeof(FluentProgressRing), new PropertyMetadata(null));



}
