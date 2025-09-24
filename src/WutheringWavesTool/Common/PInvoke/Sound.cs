using System.Text;

namespace Haiyu.Common.PInvoke;

public static class Sound
{
    [DllImport("winmm.dll", SetLastError = true)]
    static extern bool PlaySound(string pszSound, nuint hmod, uint fdwSound);

    [DllImport("winmm.dll", SetLastError = true)]
    static extern long mciSendString(
        string strCommand,
        StringBuilder strReturn,
        int iReturnLength,
        nint hwndCallback
    );

    [DllImport("winmm.dll")]
    private static extern long sndPlaySound(string lpszSoundName, long uFlags);

    [Flags]
    public enum SoundFlags
    {
        /// <summary>play synchronously (default)</summary>
        SND_SYNC = 0x0000,

        /// <summary>play asynchronously</summary>
        SND_ASYNC = 0x0001,

        /// <summary>silence (!default) if sound not found</summary>
        SND_NODEFAULT = 0x0002,

        /// <summary>pszSound points to a memory file</summary>
        SND_MEMORY = 0x0004,

        /// <summary>loop the sound until next sndPlaySound</summary>
        SND_LOOP = 0x0008,

        /// <summary>don’t stop any currently playing sound</summary>
        SND_NOSTOP = 0x0010,

        /// <summary>Stop Playing Wave</summary>
        SND_PURGE = 0x40,

        /// <summary>don’t wait if the driver is busy</summary>
        SND_NOWAIT = 0x00002000,

        /// <summary>name is a registry alias</summary>
        SND_ALIAS = 0x00010000,

        /// <summary>alias is a predefined id</summary>
        SND_ALIAS_ID = 0x00110000,

        /// <summary>name is file name</summary>
        SND_FILENAME = 0x00020000,

        /// <summary>name is resource name or atom</summary>
        SND_RESOURCE = 0x00040004,
    }

    public static void PlayClick()
    {
        string soundFile = AppDomain.CurrentDomain.BaseDirectory + "Assets\\clickSound2.wav";
        PlaySound(
            soundFile,
            nuint.Zero,
            (uint)SoundFlags.SND_FILENAME | (uint)SoundFlags.SND_ASYNC
        );
    }
}
