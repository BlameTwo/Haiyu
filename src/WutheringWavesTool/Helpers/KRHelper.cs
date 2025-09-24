namespace Haiyu.Helpers;

public static class KRHelper
{
    /// <summary>
    /// 异或加密，key99
    /// </summary>
    /// <param name="data"></param>
    /// <param name="xorKey"></param>
    /// <returns></returns>
    public static byte[]? Xor(byte[] data, byte xorKey)
    {
        if (data == null)
        {
            return null;
        }
        if (data.Length == 0)
        {
            return data;
        }
        byte[] array = new byte[data.Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)(data[i] ^ xorKey);
        }
        return array;
    }
}
