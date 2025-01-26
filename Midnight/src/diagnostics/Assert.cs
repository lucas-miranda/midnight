
namespace Midnight.Diagnostics;

public static class Assert {
    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void True(bool condition) {
        System.Diagnostics.Debug.Assert(condition, "Value is expected to be true.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void True(bool condition, string message) {
        System.Diagnostics.Debug.Assert(condition, message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void True(bool condition, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(condition, message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void False(bool condition) {
        System.Diagnostics.Debug.Assert(!condition, "Value is expected to be false.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void False(bool condition, string message) {
        System.Diagnostics.Debug.Assert(!condition, message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void False(bool condition, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(!condition, message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void NotNull<T>(T something) {
        System.Diagnostics.Debug.Assert(something != null, "Value is expected to not be null.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void NotNull<T>(T something, string message) {
        System.Diagnostics.Debug.Assert(something != null, message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void NotNull<T>(T something, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(something != null, message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Null<T>(T something) {
        System.Diagnostics.Debug.Assert(something == null, "Value is expected to be null.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Null<T>(T something, string message) {
        System.Diagnostics.Debug.Assert(something == null, message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Null<T>(T something, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(something == null, message, detailedMessage);
    }
}
