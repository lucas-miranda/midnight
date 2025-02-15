
namespace Midnight.Logging;

[System.Flags]
public enum LogStyleModes {
    None            = 0,
    Bold            = 1 << 0,
    Faint           = 1 << 1,
    Italic          = 1 << 2,
    Underline       = 1 << 3,
    Blinking        = 1 << 4,
    Reverse         = 1 << 6,
    Hidden          = 1 << 7,
    Strikethrough   = 1 << 8,
}

public struct LogSectionStyle {
    public Color? Foreground;
    public Color? Background;
    public LogStyleModes Modes;
}
