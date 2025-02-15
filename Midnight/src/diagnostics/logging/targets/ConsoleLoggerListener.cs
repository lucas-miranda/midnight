using System.Collections.Generic;
using System.Text;

namespace Midnight.Logging;

public class ConsoleLoggerListener : TextLoggerListener {
    public enum ColorResolution {
        None = 0,
        Bits16,
        Bits256,
        Truecolor,
    }

    private static readonly Dictionary<System.ConsoleColor, int> _ansi;

    private StringBuilder _builder;

    static ConsoleLoggerListener() {
        _ansi = new() {
            { System.ConsoleColor.Black,        30 },
            { System.ConsoleColor.DarkRed,      31 },
            { System.ConsoleColor.DarkGreen,    32 },
            { System.ConsoleColor.DarkYellow,   33 },
            { System.ConsoleColor.DarkBlue,     34 },
            { System.ConsoleColor.DarkMagenta,  35 },
            { System.ConsoleColor.DarkCyan,     36 },
            { System.ConsoleColor.DarkGray,     37 },
            { System.ConsoleColor.Gray,         90 },
            { System.ConsoleColor.Red,          91 },
            { System.ConsoleColor.Green,        92 },
            { System.ConsoleColor.Yellow,       93 },
            { System.ConsoleColor.Blue,         94 },
            { System.ConsoleColor.Magenta,      95 },
            { System.ConsoleColor.Cyan,         96 },
            { System.ConsoleColor.White,        97 },
        };
    }

    public ConsoleLoggerListener() : base(System.Console.Out) {
        _builder = new();

        try {
            System.Console.OutputEncoding = new UTF8Encoding();
        } catch (System.Exception) {
            // do nothing
        }
    }

    public ColorResolution Resolution { get; set; } = ColorResolution.Bits16;

    public override void Send(LogMessage message) {
        // HH:mm:ss  channel  cat D message

        Write(
            message.DateTime.ToString("HH:mm:ss"),
            new LogSectionStyle {
                Foreground = ConsoleColor.BrightBlack,
            }
        );

        if (message.IsCustomChannel) {
            Write("  ");
            Write(
                message.Channel,
                new LogSectionStyle {
                    Foreground = ConsoleColor.BrightBlack,
                }
            );
        }

        Write("  ");
        Write(
            message.Category.ToString(),
            new LogSectionStyle {
                Foreground = ConsoleColor.Blue,
            }
        );

        if (message.IsDebug) {
            Write(" ");
            Write(
                "D",
                new LogSectionStyle {
                    Foreground = ConsoleColor.White,
                    Background = ConsoleColor.BrightBlack,
                    Modes = LogStyleModes.Bold,
                }
            );
            Write(" ");
        } else {
            Write("   ");
        }

        Write(message.Text);

        if (message.AppendNewLine) {
            WriteLine();
        }

        Flush();
    }

    public override void Flush() {
        Writer.Write(_builder.ToString());
        Writer.Flush();
        _builder.Clear();
    }

    protected override void Write(string message, LogSectionStyle? style = null) {
        InternalWrite(message, style, false);
    }

    protected override void WriteLine(string message = null, LogSectionStyle? style = null) {
        InternalWrite(message, style, true);
    }

    private void InternalWrite(string message, LogSectionStyle? style, bool newLine) {
        if (!style.HasValue || Resolution == ColorResolution.None) {
            if (newLine) {
                _builder.AppendLine(message);
            } else {
                _builder.Append(message);
            }

            return;
        }

        switch (Resolution) {
            case ColorResolution.Bits16:
                {
                    _builder.Append("\x1b[");
                    bool hasStyle = false;
                    BitTag modes = style.Value.Modes;

                    foreach ((int Bit, BitTag Mode) entry in modes.IterBits()) {
                        _builder.Append(entry.Bit + 1);
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (style.Value.Foreground.HasValue) {
                        _builder.Append(GetColor16AnsiCode(style.Value.Foreground.Value));
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (style.Value.Background.HasValue) {
                        _builder.Append(GetColor16AnsiCode(style.Value.Background.Value, true));
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (hasStyle) {
                        // remove last ';'
                        _builder.Remove(_builder.Length - 1, 1);
                    }

                    _builder.Append("m");
                    _builder.Append(message);
                    _builder.Append("\x1b[0m");
                }
                break;

            case ColorResolution.Bits256:
                {
                    _builder.Append("\x1b[");
                    bool hasStyle = false;
                    BitTag modes = style.Value.Modes;

                    foreach ((int Bit, BitTag Mode) entry in modes.IterBits()) {
                        _builder.Append(entry.Bit + 1);
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (style.Value.Foreground.HasValue) {
                        _builder.Append("38;5;");
                        _builder.Append(GetColor256AnsiCode(style.Value.Foreground.Value));
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (style.Value.Background.HasValue) {
                        _builder.Append("48;5;");
                        _builder.Append(GetColor256AnsiCode(style.Value.Background.Value));
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (hasStyle) {
                        // remove last ';'
                        _builder.Remove(_builder.Length - 1, 1);
                    }

                    _builder.Append("m");
                    _builder.Append(message);
                    _builder.Append("\x1b[0m");
                }
                break;

            case ColorResolution.Truecolor:
                {
                    _builder.Append("\x1b[");
                    bool hasStyle = false;
                    BitTag modes = style.Value.Modes;

                    foreach ((int Bit, BitTag Mode) entry in modes.IterBits()) {
                        _builder.Append(entry.Bit + 1);
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (style.Value.Foreground.HasValue) {
                        _builder.Append("38;2;");
                        Color c = style.Value.Foreground.Value;
                        _builder.Append(c.R);
                        _builder.Append(";");
                        _builder.Append(c.G);
                        _builder.Append(";");
                        _builder.Append(c.B);
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (style.Value.Background.HasValue) {
                        _builder.Append("48;2;");
                        Color c = style.Value.Background.Value;
                        _builder.Append(c.R);
                        _builder.Append(";");
                        _builder.Append(c.G);
                        _builder.Append(";");
                        _builder.Append(c.B);
                        _builder.Append(";");
                        hasStyle = true;
                    }

                    if (hasStyle) {
                        // remove last ';'
                        _builder.Remove(_builder.Length - 1, 1);
                    }

                    _builder.Append("m");
                    _builder.Append(message);
                    _builder.Append("\x1b[0m");
                }
                break;

            default:
                throw new System.NotImplementedException();
        }

        if (newLine) {
            _builder.AppendLine();
        }
    }

    private int GetColor16AnsiCode(Color c, bool isBackground = false) {
        return _ansi[c.ToConsole()] + (isBackground ? 10 : 0);
    }

    private int GetColor256AnsiCode(Color c) {
        if ((c.R + c.G + c.B + c.A) % 255 == 0) {
            int code = _ansi[c.ToConsole()];

            // convert to 256 format
            if (code < 40) {
                return code - 30;
            }

            return code - 82;
        }

        float stepSize;

        if (c.R == c.G && c.G == c.B) {
            // grayscale
            stepSize = 255f / 23f;
            return 232 + Math.Clamp(Math.RoundI(c.R / stepSize), 0, 23);
        }

        stepSize = 255f / 5f;
        int r = Math.Clamp(Math.RoundI(c.R / stepSize), 0, 5),
            g = Math.Clamp(Math.RoundI(c.G / stepSize), 0, 5),
            b = Math.Clamp(Math.RoundI(c.B / stepSize), 0, 5);

        return 16 + 36 * r + 6 * g + b;
    }
}
