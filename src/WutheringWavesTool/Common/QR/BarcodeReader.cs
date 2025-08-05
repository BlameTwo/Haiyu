// 为了访问 SoftwareBitmap 的底层数据，我们需要定义 COM Interop 接口
using Microsoft.Graphics.Canvas;
using Windows.Graphics.Imaging;
using ZXing;
namespace WutheringWavesTool.Common.QR;

public sealed class CanvasBitmapLuminanceSource : LuminanceSource
{
    private readonly byte[] _luminances;
    private readonly uint _width;
    private readonly uint _height;

    public CanvasBitmapLuminanceSource(CanvasBitmap canvasBitmap)
        : base((int)canvasBitmap.SizeInPixels.Width, (int)canvasBitmap.SizeInPixels.Height)
    {
        _width = canvasBitmap.SizeInPixels.Width;
        _height = canvasBitmap.SizeInPixels.Height;

        // 获取 CanvasBitmap 的像素数据
        var pixelBytes = canvasBitmap.GetPixelBytes();

        // 创建灰度图像数据数组
        _luminances = new byte[_width * _height];

        // 将像素数据转换为灰度值
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                int sourceIndex = (int)((y * _width + x) * 4); // 假设是 BGRA8 格式
                byte b = pixelBytes[sourceIndex];
                byte g = pixelBytes[sourceIndex + 1];
                byte r = pixelBytes[sourceIndex + 2];

                // 使用加权平均法计算灰度值
                _luminances[y * _width + x] = (byte)((r * 0.299 + g * 0.587 + b * 0.114));
            }
        }
    }

    public override byte[] getRow(int y, byte[] row)
    {
        if (y < 0 || y >= Height)
        {
            throw new ArgumentException("Row is not valid");
        }
        if (row == null || row.Length < Width)
        {
            row = new byte[Width];
        }
        Array.Copy(_luminances, y * Width, row, 0, Width);
        return row;
    }

    public override byte[] Matrix => _luminances;
}