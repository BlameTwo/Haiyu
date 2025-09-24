using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.Graphics.Canvas;
using ColorHelper = CommunityToolkit.WinUI.Helpers.ColorHelper;

namespace Haiyu.Helpers;

/// <summary>
/// 图片调色
/// </summary>
public class ImageColorPaletteHelper
{
    CanvasDevice device = new CanvasDevice();

    public async Task<Color?> GetPaletteImage(Uri uri)
    {
        try
        {
            var bimap = await CanvasBitmap.LoadAsync(device, uri);

            Color[] colors = bimap.GetPixelColors();
            return await GetThemeColor(colors);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Color?> GetPaletteImage(IRandomAccessStream stream)
    {
        try
        {
            //实例化资源
            var bimap = await CanvasBitmap.LoadAsync(device, stream);

            //取色
            Color[] colors = bimap.GetPixelColors();
            return await GetThemeColor(colors);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<Color> GetThemeColor(Color[] colors)
    {
        Color color = new Color();
        await Task.Run(() =>
        {
            double sumS = 0;
            double sumV = 0;
            double sumHue = 0;
            double maxV = 0;
            double maxS = 0;
            double maxH = 0;
            double count = 0;
            List<Color> notBlackWhite = new List<Color>();
            foreach (var item in colors)
            {
                HsvColor hsv = ColorHelper.ToHsv(item);

                if (hsv.V < 0.3 || hsv.S < 0.2)
                {
                    continue;
                }
                maxS = hsv.S > maxS ? hsv.S : maxS;
                maxV = hsv.V > maxV ? hsv.V : maxV;
                maxH = hsv.H > maxH ? hsv.H : maxH;
                sumHue += hsv.H;
                sumS += hsv.S;
                sumV += hsv.V;
                count++;
                notBlackWhite.Add(item);
            }

            double avgH = sumHue / count;
            double avgV = sumV / count;
            double avgS = sumS / count;
            double maxAvgV = maxV / 2;
            double maxAvgS = maxS / 2;
            double maxAvgH = maxH / 2;
            double h = Math.Max(maxAvgV, avgV);
            double s = Math.Min(maxAvgS, avgS);
            double hue = Math.Min(maxAvgH, avgH);
            double R = 0;
            double G = 0;
            double B = 0;
            count = 0;
            foreach (var item in notBlackWhite)
            {
                HsvColor hsv = ColorHelper.ToHsv(item);
                if (hsv.H >= hue + 10 && hsv.V >= h && hsv.S >= s)
                {
                    R += item.R;
                    G += item.G;
                    B += item.B;
                    count++;
                }
            }
            double r = R / count;
            double g = G / count;
            double b = B / count;

            color = Color.FromArgb(255, (byte)r, (byte)g, (byte)b);
        });

        colors = null;

        return color;
    }

    public Color? GetShadowColor(Color? mainColor)
    {
        try
        {
            if (mainColor == null)
                return null;
            HslColor hsl = mainColor.Value.ToHsl();

            double baseFactor = hsl.L > 0.5 ? 0.6 : 0.7;
            double newL = hsl.L * baseFactor;

            newL = Math.Max(Math.Min(newL, 1), 0);

            return ColorHelper.FromHsl(hsl.H, hsl.S, newL);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    double GetRelativeLuminance(Color color)
    {
        double r = color.R / 255.0;
        double g = color.G / 255.0;
        double b = color.B / 255.0;

        // Gamma 校正
        r = (r <= 0.03928) ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
        g = (g <= 0.03928) ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
        b = (b <= 0.03928) ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

        return 0.2126 * r + 0.7152 * g + 0.0722 * b;
    }

    public Color? GetForegroundColor(Color? backgroundColor)
    {
        try
        {
            if (backgroundColor == null)
                return null;
            var luminance = backgroundColor.Value.ToHsl();
            return luminance.L > 0.5 ? Colors.White : Colors.Black;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
