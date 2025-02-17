namespace Midnight;

public static class Path {
    public static readonly char Separator = System.IO.Path.DirectorySeparatorChar,
                                AltSeparator = System.IO.Path.AltDirectorySeparatorChar;

    public static readonly char[] Separators, InvalidPathChars, InvalidFileNameChars;

    static Path() {
        char universalPathSeparator = '/';

        if (!(Separator == universalPathSeparator || AltSeparator == universalPathSeparator)) {
            if (Separator != AltSeparator) {
                Separators = new char[] {
                    Separator,
                    AltSeparator,
                    '/',
                };
            } else {
                Separators = new char[] {
                    Separator,
                    '/',
                };
            }
        } else if (Separator != AltSeparator) {
            Separators = new char[] {
                Separator,
                AltSeparator,
            };
        } else {
            Separators = new char[] {
                Separator,
            };
        }

        InvalidPathChars = System.IO.Path.GetInvalidPathChars();
        InvalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
    }

    public static bool IsSeparator(char c) {
        for (int i = 0; i < Separators.Length; i++) {
            if (c == Separators[i]) {
                return true;
            }
        }

        return false;
    }

    public static bool IsValidChar(char c) {
        foreach (char invalidChar in InvalidPathChars) {
            if (c == invalidChar) {
                return false;
            }
        }

        return true;
    }

    public static bool IsValid(string path) {
        foreach (char c in path) {
            if (!IsValidChar(c)) {
                return false;
            }
        }

        return true;
    }
}
