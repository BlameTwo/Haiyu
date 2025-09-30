using Haiyu.Plugin.Extensions;
using System.Text.RegularExpressions;
using System.Threading;
using WinUIEx.Messaging;

namespace Haiyu.Services.Contracts;

public interface IScreenCaptureService
{
    public (bool,string) Register();
    public void Unregister();
}
