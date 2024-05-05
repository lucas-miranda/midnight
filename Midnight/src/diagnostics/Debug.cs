
namespace Midnight.Diagnostics;

public static class Debug {
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Assert(bool condition) {
        System.Diagnostics.Debug.Assert(condition);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void Assert(bool condition, string message) {
        System.Diagnostics.Debug.Assert(condition, message);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void Assert(bool condition, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(condition, message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void AssertNotNull<T>(T something) {
        System.Diagnostics.Debug.Assert(something != null);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void AssertNotNull<T>(T something, string message) {
        System.Diagnostics.Debug.Assert(something != null, message);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void AssertNotNull<T>(T something, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(something != null, message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void AssertIsNull<T>(T something) {
        System.Diagnostics.Debug.Assert(something == null);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void AssertIsNull<T>(T something, string message) {
        System.Diagnostics.Debug.Assert(something == null, message);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    public static void AssertIsNull<T>(T something, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(something == null, message, detailedMessage);
    }
}
