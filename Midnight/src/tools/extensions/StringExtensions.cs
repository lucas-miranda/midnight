namespace Midnight;

public static class StringExtensions {
    public static bool IsEmpty(this string c) {
        return c.Length <= 0;
    }
}
