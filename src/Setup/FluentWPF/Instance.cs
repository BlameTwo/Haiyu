
#region 全局引用

global using FluentWPF.Common;
global using FluentWPF.Controls;
global using FluentWPF.Models.Enums;
global using System.Runtime.InteropServices;
global using System.Windows;
global using System.Windows.Interop;
global using static FluentWPF.Common.Methods;
global using System.Windows.Controls.Primitives;
using FluentWPF.Models;
#endregion
namespace FluentWPF;

public delegate void InstanceLogOutputHandler(object sender, UILogModel message);

public static class Instance
{

    static Instance()
    {
        _windowList = new();
    }
    public static Application FluentApp { get; private set; }

    internal static int Theme = 0;
    internal static int SysteBackdrop = 0;

    private static Dictionary<Guid, FluentWindow> _windowList;

    internal static InstanceLogOutputHandler InstanceLogOutputHandler;

    public static event InstanceLogOutputHandler InstanceLogOutputEvent
    {
        add => InstanceLogOutputHandler += value;
        remove => InstanceLogOutputHandler -= value;
    }

    /// <summary>
    /// 调用AddWindow，以启动系统笔刷调度
    /// </summary>
    /// <param name="win"></param>
    /// <param name="callBack"></param>
    public static void AddWindow(FluentWindow win,Action<Guid> callBack)
    {
        Guid guid = Guid.NewGuid();
        _windowList.Add(guid,win);
        callBack.Invoke(guid);
    }

    public static void RemoveWindow(Guid guid)
    {
        var result = _windowList.TryGetValue(guid, out var fluent);
        if (result)
        {
            _windowList.Remove(guid);
        }
    }

    /// <summary>
    /// 初始化主题，不一定需要这样的注册方式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void InitTheme<App>(App app)
        where App :Application
    {
        FluentApp = app;
    }

    public static void ApplyTheme(ThemeTypeEnum type)
    {
        foreach (var item in FluentApp.Resources.MergedDictionaries)
        {
            if(item is FluentXamlResource resouce)
            {
                resouce.Type = type;
                Theme = (int)type;
                UpdateTheme();
                break;
            }
        }
    }


    internal static void UpdateTheme()
    {
        if (FluentApp == null)
            return;
        foreach (var win in _windowList)
        {
            Methods.DwmSetWindowAttribute(new WindowInteropHelper(win.Value).Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref Theme, Marshal.SizeOf(typeof(int)));
            Methods.DwmSetWindowAttribute(new WindowInteropHelper(win.Value).Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref SysteBackdrop, Marshal.SizeOf(typeof(int)));
        }
    }

    public static void UpdateSystemdrop(SystemBackdropBase type)
    {
        if (FluentApp == null)
            return;
        var drop = (int)type.SystemBackdropTypeEnum;
        foreach (var win in _windowList)
        {
            Methods.DwmSetWindowAttribute(new WindowInteropHelper(win.Value).Handle, (int)DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref drop, Marshal.SizeOf(typeof(int)));
        }
    }
}