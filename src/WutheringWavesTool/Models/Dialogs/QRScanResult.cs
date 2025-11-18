namespace Haiyu.Models.Dialogs
{
    public class QRScanResult
    {
        public bool Result { get; set; } = false;

        public QRScanResult(bool result)
        {
            Result = result;
        }

        public QRScanResult()
        {

        }
    }
}
