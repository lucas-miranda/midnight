
namespace Midnight.Diagnostics;

public static class Assert {
    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void True(bool condition) {
        True(condition, "Value is expected to be true.");
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
        False(condition, "Value is expected to be false.");
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
        NotNull<T>(something, "Value is expected to not be null.");
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
        Null<T>(something, "Value is expected to be null.");
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

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Is<T>(object something) {
        Is<T>(something, $"Value is expected to have type {typeof(T).Name}.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Is<T>(object something, string message) {
        System.Diagnostics.Debug.Assert(something is T, message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Is<T>(object something, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(something is T, message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void IsNot<T>(object something) {
        IsNot<T>(something, $"Value is expected to not have type {typeof(T).Name}.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void IsNot<T>(object something, string message) {
        System.Diagnostics.Debug.Assert(!(something is T), message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void IsNot<T>(object something, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(!(something is T), message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Is<T>(System.Type type) {
        Is<T>(type, $"Type {type.Name} is expected to be assignable to type {typeof(T).Name}.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Is<T>(System.Type type, string message) {
        System.Diagnostics.Debug.Assert(typeof(T).IsAssignableFrom(type), message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void Is<T>(System.Type type, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(typeof(T).IsAssignableFrom(type), message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void IsNot<T>(System.Type type) {
        IsNot<T>(type, $"Type {type.Name} is expected to not be assignable to type {typeof(T).Name}.");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void IsNot<T>(System.Type type, string message) {
        System.Diagnostics.Debug.Assert(!typeof(T).IsAssignableFrom(type), message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void IsNot<T>(System.Type type, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(!typeof(T).IsAssignableFrom(type), message, detailedMessage);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void InRange(int value, int min, int max) {
        InRange(value, min, max, $"Value ({value}) is expected to be in range [{min}, {max}].");
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void InRange(int value, int min, int max, string message) {
        System.Diagnostics.Debug.Assert(!(value < min || value > max), message);
    }

    [System.Diagnostics.Conditional("DEBUG"),
     System.Diagnostics.Conditional("ASSERTIONS")]
    public static void InRange(int value, int min, int max, string message, string detailedMessage) {
        System.Diagnostics.Debug.Assert(!(value < min || value > max), message, detailedMessage);
    }
}
