using Microsoft.Graphics.Canvas;

namespace Haiyu.Helpers
{
    public class OctreeColorExtractor
    {
        // 八叉树根节点
        private OctreeNode root;
        public const int MAX_DEPTH = 8; // 八叉树最大深度（对应8位RGB精度）

        public async Task<Tuple<Color?, List<Color>>?> GetThemeColorAsync(
            IRandomAccessStream? imageFile
        )
        {
            if (imageFile == null)
                throw new ArgumentNullException(nameof(imageFile));

            var colors = await ExtractColorsFromImageAsync(imageFile);
            if (colors.Count == 0)
                return new(Colors.Red, []);

            root = new OctreeNode();
            foreach (var color in colors)
            {
                root.Insert(color);
            }

            while (root.LeafCount > 8)
            {
                root.MergeLeastSignificant();
            }

            var palette = root.GetPalette();

            var scoredColors = palette
                .Select(c => new
                {
                    Color = c,
                    Score = (c.GetSaturation() * 1.5)
                        + (c.GetBrightness() >= 0.3 && c.GetBrightness() <= 0.7 ? 1 : 0)
                        + (Math.Log(root.GetColorCount(c) + 1)),
                })
                .OrderByDescending(c => c.Score)
                .ThenByDescending(c => c.Color.GetSaturation())
                .ThenByDescending(c => c.Color.GetBrightness())
                .ToList();

            var bestColor = scoredColors.FirstOrDefault()?.Color;
            if (bestColor == default)
            {
                bestColor = palette.OrderByDescending(c => root.GetColorCount(c)).FirstOrDefault();
            }

            return new(bestColor, scoredColors.Select(x => x.Color).ToList());
        }

        private async Task<List<Color>> ExtractColorsFromImageAsync(IRandomAccessStream imageFile)
        {
            var colors = new List<Color>();
            CanvasDevice device = new CanvasDevice();
            CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(device, imageFile);
            //int step = CalculateSamplingStep(
            //    (int)bitmap.SizeInPixels.Width,
            //    (int)bitmap.SizeInPixels.Height
            //);
            int step = 300;
            for (int y = 0; y < bitmap.SizeInPixels.Height; y += step)
            {
                for (int x = 0; x < bitmap.SizeInPixels.Width; x += step)
                {
                    Color pixelColor = bitmap.GetPixelColors(x, y, 1, 1)[0];
                    colors.Add(pixelColor);
                }
            }
            return colors;
        }

        // 动态计算采样步长
        private int CalculateSamplingStep(int width, int height)
        {
            double totalPixels = width * height;
            const int targetSampleCount = 10000;
            return (int)Math.Max(1, Math.Sqrt(totalPixels / targetSampleCount));
        }
    }
}
