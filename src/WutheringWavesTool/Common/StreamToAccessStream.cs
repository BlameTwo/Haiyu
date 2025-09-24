namespace Haiyu.Common;

public static class StreamToAccessStream
{
    public static async Task<IRandomAccessStream> ConvertStreamToRandomAccessStream(
        this Stream inputStream
    )
    {
        inputStream.Seek(0, SeekOrigin.Begin);
        InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
        Stream outputStream = randomAccessStream.AsStreamForRead();
        await inputStream.CopyToAsync(outputStream);
        randomAccessStream.Seek(0);
        return randomAccessStream;
    }
}
