namespace Haiyu.Helpers
{
    public static class ColorExtensions
    {
        public static double GetBrightness(this Color color)
        {
            return (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) / 255.0;
        }

        public static double GetSaturation(this Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;
            return delta == 0 ? 0 : delta / max;
        }
    }
}
