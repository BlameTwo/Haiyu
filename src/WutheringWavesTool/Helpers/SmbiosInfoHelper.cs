using System;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;
namespace WutheringWavesTool.Helpers;
public class HardwareIdGenerator
{
    // ==================== API 声明 ====================

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool DeviceIoControl(
        IntPtr hDevice,
        uint dwIoControlCode,
        IntPtr lpInBuffer,
        uint nInBufferSize,
        IntPtr lpOutBuffer,
        uint nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    // ==================== 常量 ====================
    private const uint GENERIC_READ = 0x80000000;
    private const uint FILE_SHARE_READ = 1;
    private const uint FILE_SHARE_WRITE = 2;
    private const uint OPEN_EXISTING = 3;
    private const uint IOCTL_STORAGE_QUERY_PROPERTY = 0x002D1400;

    [StructLayout(LayoutKind.Sequential)]
    private struct STORAGE_PROPERTY_QUERY
    {
        public uint PropertyId;
        public uint QueryType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] AdditionalParameters;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct STORAGE_DEVICE_DESCRIPTOR
    {
        public uint Version;
        public uint Size;
        public ushort PhysicalBytesPerSector;
        public ushort BytesPerPhysicalSector;
        public ushort BytesPerLogicalSector;
        public ushort BytesPerPhysicalSectorForDevice;
        public uint DeviceType;
        public uint DeviceTypeModifier;
        public bool RemovableMedia;
        public bool CommandQueueing;
        public uint VendorIdOffset;
        public uint ProductIdOffset;
        public uint ProductRevisionOffset;
        public uint SerialNumberOffset;
        // ... 其他字段
    }

    // ==================== 核心方法 ====================

    public static string GenerateUniqueId()
    {
        try
        {
            string diskSerial = GetHardDiskSerial();
            string cpuId = GetCpuId(); // 注意：CPUID 通常需内联汇编，此处简化为模拟

            // 拼接所有硬件信息
            string combined = $"{diskSerial}|{cpuId}";

            // 使用 SHA-1 生成 40 位哈希
            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(combined));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString(); // 大写十六进制
            }
        }
        catch
        {
            return "UNKNOWN_HARDWARE_ID";
        }
    }

    // ==================== 获取硬盘序列号 ====================
    private static string GetHardDiskSerial()
    {
        IntPtr hDevice = CreateFile(
            @"\\.\PhysicalDrive0",
            GENERIC_READ,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            IntPtr.Zero,
            OPEN_EXISTING,
            0,
            IntPtr.Zero);

        if (hDevice == IntPtr.Zero || hDevice.ToInt64() == -1)
            return "NO_DISK_ACCESS";

        try
        {
            STORAGE_PROPERTY_QUERY query = new STORAGE_PROPERTY_QUERY
            {
                PropertyId = 0, // StorageDeviceProperty
                QueryType = 0
            };

            int querySize = Marshal.SizeOf(query);
            IntPtr queryPtr = Marshal.AllocHGlobal(querySize);
            Marshal.StructureToPtr(query, queryPtr, false);

            int bufferSize = 1024;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
            uint bytesReturned;

            bool result = DeviceIoControl(
                hDevice,
                IOCTL_STORAGE_QUERY_PROPERTY,
                queryPtr,
                (uint)querySize,
                buffer,
                (uint)bufferSize,
                out bytesReturned,
                IntPtr.Zero);

            if (result)
            {
                STORAGE_DEVICE_DESCRIPTOR deviceDesc = (STORAGE_DEVICE_DESCRIPTOR)Marshal.PtrToStructure(buffer, typeof(STORAGE_DEVICE_DESCRIPTOR));
                if (deviceDesc.SerialNumberOffset > 0)
                {
                    IntPtr serialPtr = IntPtr.Add(buffer, (int)deviceDesc.SerialNumberOffset);
                    string serial = Marshal.PtrToStringAnsi(serialPtr);
                    return serial?.Trim();
                }
            }
        }
        finally
        {
            CloseHandle(hDevice);
        }

        return "UNKNOWN_SERIAL";
    }

    // ==================== 模拟获取 CPU ID（简化）====================
    // 注意：真正的 CPUID 需要 x86 汇编或外部库，此处用环境信息代替
    private static string GetCpuId()
    {
        // 在真实场景中，可通过 __cpuid 或 Native CPUID 调用获取
        // 这里用处理器数量和系统信息模拟
        return Environment.ProcessorCount.ToString();
    }
}