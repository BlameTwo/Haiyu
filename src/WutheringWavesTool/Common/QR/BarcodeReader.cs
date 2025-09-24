using Microsoft.Graphics.Canvas;
using Windows.Graphics.Imaging;
using ZXing;
namespace Haiyu.Common.QR;

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

        var pixelBytes = canvasBitmap.GetPixelBytes();

        _luminances = new byte[_width * _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                int sourceIndex = (int)((y * _width + x) * 4);
                byte b = pixelBytes[sourceIndex];
                byte g = pixelBytes[sourceIndex + 1];
                byte r = pixelBytes[sourceIndex + 2];
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