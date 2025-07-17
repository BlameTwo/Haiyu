using CommunityToolkit.Mvvm.ComponentModel;

namespace Waves.Core.Models;

public partial class LogMessage
{
    public DateTime DateTime { get; set; }

    public string Message { get; set; }
}
