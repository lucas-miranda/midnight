
namespace Midnight.Logging;

public static class ConsoleColor {
    // alpha is used as bright bit
    public static Color Black = new Color(0, 0, 0, 0),
                        Blue = new Color(0, 0, 255, 0),
                        Green = new Color(0, 255, 0, 0),
                        Cyan = new Color(0, 255, 255, 0),
                        Red = new Color(255, 0, 0, 0),
                        Magenta = new Color(255, 0, 255, 0),
                        Yellow = new Color(255, 255, 0, 0),
                        White = new Color(255, 255, 255, 0),
                        BrightBlack = new Color(0, 0, 0, 255),
                        BrightBlue = new Color(0, 0, 255, 255),
                        BrightGreen = new Color(0, 255, 0, 255),
                        BrightCyan = new Color(0, 255, 255, 255),
                        BrightRed = new Color(255, 0, 0, 255),
                        BrightMagenta = new Color(255, 0, 255, 255),
                        BrightYellow = new Color(255, 255, 0, 255),
                        BrightWhite = new Color(255, 255, 255, 255);
}
