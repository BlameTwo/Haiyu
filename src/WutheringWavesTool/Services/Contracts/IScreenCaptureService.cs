namespace Haiyu.Services.Contracts;

public interface IScreenCaptureService
{
    public (bool, string) Register();
    public void Unregister();
}
