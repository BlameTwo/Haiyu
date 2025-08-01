using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiffUpdate
{
    public class SharedMemory : IDisposable
    {
        MemoryMappedFile _file;
        private SemaphoreSlim _semaphore;
        MemoryMappedViewAccessor _fileView;

        public SharedMemory(string key, int size)
        {
            try
            {
                _file = MemoryMappedFile.CreateOrOpen(key, size);
                _semaphore = new SemaphoreSlim(1, 1);
                _fileView = _file.CreateViewAccessor();
                byte[] array = new byte[size];
                _fileView.WriteArray(0L, array, 0, array.Length);
            }
            catch (Exception ex)
            {
                // 可以考虑记录异常或重新抛出
            }
        }

        public void Dispose()
        {
            _file?.Dispose();
            _fileView?.Dispose();
            _semaphore?.Dispose();
        }

        public async Task<(bool success, ulong[] data)> ReadUlongAsync(
            int offset,
            int count,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            ulong[] data = new ulong[count];

            try
            {
                bool acquired;
                if (timeout.HasValue)
                {
                    acquired = await _semaphore.WaitAsync(timeout.Value, cancellationToken);
                }
                else
                {
                    await _semaphore.WaitAsync(cancellationToken);
                    acquired = true;
                }

                if (!acquired)
                {
                    return (false, data);
                }
            }
            catch (Exception ex)
            {
                return (false, data);
            }

            try
            {
                for (int i = 0; i < count; i++)
                {
                    data[i] = _fileView.ReadUInt64(offset + i * Marshal.SizeOf<ulong>());
                }
            }
            catch (Exception ex2)
            {
                _semaphore.Release();
                return (false, data);
            }

            _semaphore.Release();
            return (true, data);
        }

        public async Task<(bool success, ulong data)> ReadUlongAsync(
            int offset,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        )
        {
            ulong data = 0uL;

            try
            {
                bool acquired;
                if (timeout.HasValue)
                {
                    acquired = await _semaphore.WaitAsync(timeout.Value, cancellationToken);
                }
                else
                {
                    await _semaphore.WaitAsync(cancellationToken);
                    acquired = true;
                }

                if (!acquired)
                {
                    return (false, data);
                }
            }
            catch (Exception ex)
            {
                return (false, data);
            }

            try
            {
                data = _fileView.ReadUInt64(offset);
            }
            catch (Exception ex2)
            {
                _semaphore.Release();
                return (false, data);
            }

            _semaphore.Release();
            return (true, data);
        }

        public bool ReadUlong(int offset, int count, out ulong[] data, TimeSpan? timeout = null)
        {
            var result = ReadUlongAsync(offset, count, timeout).GetAwaiter().GetResult();
            data = result.data;
            return result.success;
        }

        public bool ReadUlong(int offset, out ulong data, TimeSpan? timeout = null)
        {
            var result = ReadUlongAsync(offset, timeout).GetAwaiter().GetResult();
            data = result.data;
            return result.success;
        }
    }
}
