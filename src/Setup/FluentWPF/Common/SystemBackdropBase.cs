using FluentWPF.Models.Enums;
using System.Runtime.InteropServices;
using System.Windows.Controls.Primitives;

namespace FluentWPF.Common;

public abstract class SystemBackdropBase
{
    public SystemBackdropBase(SystemBackdropTypeEnum systemBackdropTypeEnum)
    {
        SystemBackdropTypeEnum = systemBackdropTypeEnum;
    }

    public SystemBackdropTypeEnum SystemBackdropTypeEnum { get; }

    public virtual void ChangedBackdrop()
    {
        Instance.InstanceLogOutputHandler?.Invoke(this, new Models.UILogModel(LogType.INFO, "更换系统笔刷"));
    }

    public void SetBackdrop(FluentWindow win)
    {
        int drop = (int)SystemBackdropTypeEnum;
        Instance.SysteBackdrop = drop;
        UpdateTheme(win);
        ChangedBackdrop();
    }
    private void UpdateTheme(FluentWindow w)
    {
        var drop = (int)SystemBackdropTypeEnum;
        Methods.DwmSetWindowAttribute(new WindowInteropHelper(w).Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref Instance.Theme, Marshal.SizeOf(typeof(int)));
        Methods.DwmSetWindowAttribute(new WindowInteropHelper(w).Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref drop, Marshal.SizeOf(typeof(int)));
    }
}
