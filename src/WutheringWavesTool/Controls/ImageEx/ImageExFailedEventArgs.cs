namespace Haiyu.Controls;

public delegate void ImageExFailedEventHandler(object sender, ImageExFailedEventArgs e);

/// <summary>
/// Provides data for the <see cref="ImageEx"/> ImageFailed event.
/// </summary>
public class ImageExFailedEventArgs : EventArgs
{
    public ImageExFailedEventArgs(Exception errorException)
    {
        ErrorException = errorException;
        ErrorMessage = ErrorException?.Message;
    }

    public Exception ErrorException { get; private set; }

    public string ErrorMessage { get; private set; }
}
