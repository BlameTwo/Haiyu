namespace Haiyu.Common;

public static class ByteConversion
{
    /// <summary>
    /// Byte To MB
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static double BytesToMegabytes(long bytes, int length = 0)
    {
        double value = bytes / 1024.0 / 1024.0;
        if (length == 0)
        {
            return value;
        }
        else
        {
            return Math.Round(value, length);
        }
    }

    /// <summary>
    /// Byte To KB
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static double BytesToKilobytes(long bytes, int length = 0)
    {
        double value = bytes / 1024.0;
        if (length == 0)
        {
            return value;
        }
        else
        {
            return Math.Round(value, length);
        }
    }

    /// <summary>
    /// Byte To GB
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static double BytesToGigabytes(long bytes, int length = 0)
    {
        double value = bytes / 1024.0 / 1024.0 / 1024.0;
        if (length == 0)
        {
            return value;
        }
        else
        {
            return Math.Round(value, length);
        }
    }
}
