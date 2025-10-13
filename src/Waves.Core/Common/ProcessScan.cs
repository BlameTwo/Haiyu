using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Waves.Core.Common;

/// <summary>
/// 进程监控与扫描
/// </summary>
public static class ProcessScan
{
    public enum CREATE_TOOLHELP_SNAPSHOT_FLAGS : uint
    {
        TH32CS_SNAPPROCESS = 0x00000002
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PROCESSENTRY32W
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS dwFlags, uint th32ProcessID);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool Process32FirstW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool Process32NextW(IntPtr hSnapshot, ref PROCESSENTRY32W lppe);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    public static bool CheckGameAliveWithWin32(string exeName, uint pid, out bool contained, out uint ppid)
    {
        contained = false;
        ppid = 0u;
        IntPtr hSnapshot = IntPtr.Zero;
        try
        {
            // 创建进程快照
            hSnapshot = CreateToolhelp32Snapshot(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPPROCESS, 0);
            if (hSnapshot == IntPtr.Zero)
                return false;

            // 初始化进程条目
            PROCESSENTRY32W lppe = new PROCESSENTRY32W
            {
                dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32W))
            };

            // 获取第一个进程
            if (!Process32FirstW(hSnapshot, ref lppe))
                return false;

            do
            {
                Debug.WriteLine(lppe.szExeFile);
                if (pid == lppe.th32ProcessID)
                {
                    contained = true;
                    ppid = lppe.th32ParentProcessID;
                    return true;
                }
            }
            while (Process32NextW(hSnapshot, ref lppe));

            return true;
        }
        finally
        {
            // 确保句柄关闭
            if (hSnapshot != IntPtr.Zero)
                CloseHandle(hSnapshot);
        }
    }
}
