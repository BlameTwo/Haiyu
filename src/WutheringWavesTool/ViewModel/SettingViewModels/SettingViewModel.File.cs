using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiyu.ViewModel;

partial class SettingViewModel
{
    const int SW_SHOWNORMAL = 1;

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr ShellExecute(
        IntPtr hwnd,
        string verb,
        string file,
        string parameters,
        string directory,
        int showCmd
    );

    [RelayCommand]
    void OpenConfigFolder()
    {
        ShellExecute(IntPtr.Zero, "open", App.BassFolder, null, null, SW_SHOWNORMAL);
    }


    [RelayCommand]
    void OpenCaptureFolder()
    {

        ShellExecute(IntPtr.Zero, "open", App.ScreenCaptures, null, null, SW_SHOWNORMAL);
    }
}
