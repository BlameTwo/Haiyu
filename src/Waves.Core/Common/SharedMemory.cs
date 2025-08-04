using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Waves.Core.Common
{

    internal class SharedMemory : IDisposable
    {
        MemoryMappedFile _file;
        private Mutex _mutex;
        MemoryMappedViewAccessor _fileView;

        public SharedMemory(string key, int size)
        {
            try
            {
                _file = MemoryMappedFile.CreateOrOpen(key, size);
                _mutex = new Mutex(initiallyOwned: false, key + "_mutex");
                _fileView = _file.CreateViewAccessor();
                byte[] array = new byte[size];
                _fileView.WriteArray(0L, array, 0, array.Length);
            }
            catch (Exception ex) { }
        }

        public void Dispose()
        {
            _file?.Dispose();
            _fileView?.Dispose();
            _mutex?.Dispose();
        }

        public bool ReadUlong(int offset, int count, out ulong[] data, TimeSpan? timeout = null)
        {
            data = new ulong[count];
            try
            {
                bool flag = false;
                if (!(timeout.HasValue ? _mutex.WaitOne(timeout.Value) : _mutex.WaitOne()))
                {
                    return false;
                }
            }
            catch (Exception ex) { }
            try
            {
                for (int i = 0; i < count; i++)
                {
                    data[i] = _fileView.ReadUInt64(offset + i * Marshal.SizeOf<ulong>());
                }
            }
            catch (Exception ex2)
            {
                _mutex.ReleaseMutex();

                return false;
            }
            _mutex.ReleaseMutex();
            return true;
        }

        public bool ReadUlong(int offset, out ulong data, TimeSpan? timeout = null)
        {
            data = 0uL;
            try
            {
                bool flag = false;
                if (!(timeout.HasValue ? _mutex.WaitOne(timeout.Value) : _mutex.WaitOne()))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            try
            {
                data = _fileView.ReadUInt64(offset);
            }
            catch (Exception ex2)
            {
                _mutex.ReleaseMutex();

                return false;
            }
            _mutex.ReleaseMutex();
            return true;
        }
    }
}
