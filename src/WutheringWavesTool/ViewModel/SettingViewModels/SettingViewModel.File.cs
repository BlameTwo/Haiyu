using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiyu.ViewModel;

partial class SettingViewModel
{
    [ObservableProperty]
    public partial string WebViewVersion { get; set; }

    [ObservableProperty]
    public partial string WindowsAppSdkVersion { get; set; }

    [ObservableProperty]
    public partial string RunType { get; set; }

    [ObservableProperty]
    public partial string FrameworkType { get; set; }

    void GetAllVersion()
    {
        WebViewVersion = CoreWebView2Environment.GetAvailableBrowserVersionString()??"未安装";
        this.WindowsAppSdkVersion = $"{Microsoft.WindowsAppSDK.Release.Major}.{Microsoft.WindowsAppSDK.Release.Minor}.{Microsoft.WindowsAppSDK.Release.Patch} {Microsoft.WindowsAppSDK.Release.Channel}";
        this.RunType = RuntimeFeature.IsDynamicCodeCompiled ? "JIT" : "AOT";
        this.FrameworkType = RuntimeInformation.FrameworkDescription;
    }

    [RelayCommand]
    void OpenConfigFolder()
    {
        WindowExtension.ShellExecute(IntPtr.Zero, "open", App.BassFolder, null, null, WindowExtension.SW_SHOWNORMAL);
    }

    [RelayCommand]
    void OpenCaptureFolder()
    {
        WindowExtension.ShellExecute(IntPtr.Zero, "open", App.ScreenCaptures, null, null, WindowExtension.SW_SHOWNORMAL);
    }

    
}
