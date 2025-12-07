using System.Collections.Generic;

namespace Haiyu.Plugin.Extensions;

public class Keys
{
    public string Name { get; set; }
    public int Value { get; set; }

    public Keys(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public static List<Keys> GetDefault()
    {
        return new List<Keys>
        {
            new Keys("F1", 0x70),
            new Keys("F2", 0x71),
            new Keys("F3", 0x72),
            new Keys("F4", 0x73),
            new Keys("F5", 0x74),
            new Keys("F6", 0x75),
            new Keys("F7", 0x76),
            new Keys("F8", 0x77),
            new Keys("F9", 0x78),
            new Keys("F10", 0x79),
            new Keys("F11", 0x7A),
            new Keys("F12", 0x7B),
            new Keys("A", 0x41),
            new Keys("B", 0x42),
            new Keys("C", 0x43),
            new Keys("D", 0x44),
            new Keys("E", 0x45),
            new Keys("F", 0x46),
            new Keys("G", 0x47),
            new Keys("H", 0x48),
            new Keys("I", 0x49),
            new Keys("J", 0x4A),
            new Keys("K", 0x4B),
            new Keys("L", 0x4C),
            new Keys("M", 0x4D),
            new Keys("N", 0x4E),
            new Keys("O", 0x4F),
            new Keys("P", 0x50),
            new Keys("Q", 0x51),
            new Keys("R", 0x52),
            new Keys("S", 0x53),
            new Keys("T", 0x54),
            new Keys("U", 0x55),
            new Keys("V", 0x56),
            new Keys("W", 0x57),
            new Keys("X", 0x58),
            new Keys("Y", 0x59),
            new Keys("Z", 0x5A),
        };
    }
}

public class ModifierKey
{
    public string Name { get; set; }

    public int Value { get; set; }

    public ModifierKey(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public static List<ModifierKey> GetDefault()
    {
        return new List<ModifierKey>()
        {
            new("Shift", 0x0001),
            new("Ctrl", 0x0002),
            new("ALT", 0x0003),
            new("Win", 0x0008),
        };
    }
}
