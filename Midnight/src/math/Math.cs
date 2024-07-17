namespace Midnight;

public static class Math {
    public const float Epsilon  = 0.000001f,
                       PI       = (float) System.Math.PI,
                       DoublePI = 2.0f * PI,
                       TriplePI = 3.0f * PI,
                       HalfPI   = PI / 2.0f,
                       ThirdPI  = PI / 3.0f,
                       FourthPI = PI / 4.0f,
                       Tau      = 2.0f * PI,
                       HalfTau  = PI,
                       RadToDeg = 180.0f / PI,
                       DegToRad = PI / 180.0f;

    #region Numeric

    public static bool IsPower2(int n) {
        return (n & (n - 1)) == 0;
    }

    public static bool IsPower2(uint n) {
        return (n & (n - 1U)) == 0U;
    }

    public static bool IsPower2(long n) {
        return (n & (n - 1L)) == 0L;
    }

    public static bool IsPower2(ulong n) {
        return (n & (n - 1UL)) == 0UL;
    }

    public static int CeilPower2(int n) {
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        n++;
        n += n == 0 ? 1 : 0;
        return n;
    }

    public static uint CeilPower2(uint n) {
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        n++;
        n += n == 0 ? 1U : 0U;
        return n;
    }

    /// <summary>
    /// Ensures <paramref name="value"/> is in range [<paramref name="min"/>, <paramref name="max"/>[.
    /// </summary>
    public static float Wrap(float value, float min, float max) {
        value %= max - min;

        if (value > max) {
            return min + (value - max);
        } else if (value < min) {
            return max + (value - min);
        }

        return value;
    }

    /// <summary>
    /// Snaps <paramref name="value"/> to nearest <paramref name="step"/> multiple.
    /// </summary>
    public static float Snap(float value, float step) {
        return step * Round(value / step);
    }

    /// <summary>
    /// Clamps the given value between the given minimum float and maximum float values. Returns the given value if it is within the minimum and maximum range.
    /// </summary>
    /// <returns>
    /// The float result between the minimum and maximum values.
    /// </returns>
    public static float Clamp(float value, float min, float max) {
        return System.Math.Clamp(value, min, max);
    }

    public static int Clamp(int value, int min, int max) {
        return Min(Max(value, min), max);
    }

    public static byte Clamp(byte value, byte min, byte max) {
        return Min(Max(value, min), max);
    }

    public static float Clamp01(float value) {
        return Clamp(value, 0.0f, 1.0f);
    }

    public static float Abs(float value) {
        return System.MathF.Abs(value);
    }

    public static double Abs(double value) {
        return System.Math.Abs(value);
    }

    public static int Abs(int value) {
        return System.Math.Abs(value);
    }

    public static float Ceil(float value) {
        return System.MathF.Ceiling(value);
    }

    public static int CeilI(float value) {
        return (int) System.MathF.Ceiling(value);
    }

    public static float Floor(float value) {
        return System.MathF.Floor(value);
    }

    public static int FloorI(float value) {
        return (int) System.MathF.Floor(value);
    }

    /// <summary>Returns value rounded to the nearest integer.</summary>
    public static float Round(float value) {
        return System.MathF.Round(value);
    }

    /// <summary>Returns value rounded to the nearest integer.</summary>
    public static int RoundI(float value) {
        return (int) System.MathF.Round(value);
    }

    /// <summary>Returns the smallest of two or more values.</summary>
    public static float Min(float a, float b) {
        return System.Math.Min(a, b);
    }

    /// <summary>Returns the smallest of two or more values.</summary>
    public static int Min(int a, int b) {
        return System.Math.Min(a, b);
    }

    public static byte Min(byte a, byte b) {
        return System.Math.Min(a, b);
    }

    /// <summary>Returns largest of two or more values.</summary>
    public static float Max(float a, float b) {
        return System.Math.Max(a, b);
    }

    public static float Max(float a, float b, float c) {
        return Max(Max(a, b), c);
    }

    public static float Max(float a, float b, float c, float d) {
        return Max(Max(a, b), Max(c, d));
    }

    public static int Max(int a, int b) {
        return System.Math.Max(a, b);
    }

    public static byte Max(byte a, byte b) {
        return System.Math.Max(a, b);
    }

    public static int Sign(float value) {
        return System.Math.Sign(value);
    }

    public static float Sqrt(float n) {
        return System.MathF.Sqrt(n);
    }

    public static float Lerp(float start, float end, float t) {
        return (1.0f - t) * start + end * t;
    }

    public static float Unlerp(float start, float end, float value) {
        if (Math.Abs(end - start) < Epsilon) {
            return 1.0f;
        }

        return (value - start) / (end - start);
    }

    public static float Map(float value, float min, float max, float targetMin, float targetMax) {
        return Lerp(targetMin, targetMax, Unlerp(min, max, value));
    }

    #endregion Numeric

    #region Geometry

    public static float ToRadians(float deg) {
        return deg * DegToRad;
    }

    public static float ToDegrees(float rad) {
        return rad * RadToDeg;
    }

    public static Vector2 CartesianToPolar(Vector2 cartesianPoint) {
        return new Vector2(cartesianPoint.Length(), Angle(cartesianPoint));
    }

    public static Vector2 CartesianToPolar(float x, float y) {
        return CartesianToPolar(new Vector2(x, y));
    }

    public static Vector2 PolarToCartesian(float radial, float angular) {
        return new Vector2(radial * Cos(angular), radial * Sin(angular));
    }

    public static Vector2 PolarToCartesian(Vector2 polarPoint) {
        return PolarToCartesian(polarPoint.X, polarPoint.Y);
    }

    public static float Sin(float deg) {
        return System.MathF.Sin(deg * DegToRad);
    }

    public static float Cos(float deg) {
        return System.MathF.Cos(deg * DegToRad);
    }

    public static float Tan(float deg) {
        return System.MathF.Tan(deg * DegToRad);
    }

    public static float Angle(float x, float y) {
        return System.MathF.Atan2(y, x) * RadToDeg;
    }

    public static float Angle(Vector2 point) {
        return Angle(point.X, point.Y);
    }

    public static float Angle(float x1, float y1, float x2, float y2) {
        return System.MathF.Atan2(y2 - y1, x2 - x1) * RadToDeg;
    }

    public static float Angle(Vector2 from, Vector2 to) {
        return Angle(from.X, from.Y, to.X, to.Y);
    }

    public static float Angle(Vector2 a, Vector2 b, Vector2 c) {
        float  aX = a.X, aY = a.Y,
               bX = b.X, bY = b.Y,
               cX = c.X, cY = c.Y;

        float  ba = (bX - aX) * (bX - aX) + (bY - aY) * (bY - aY),
               bc = (bX - cX) * (bX - cX) + (bY - cY) * (bY - cY),
               ca = (cX - aX) * (cX - aX) + (cY - aY) * (cY - aY);

        return System.MathF.Acos((bc + ba - ca) / Sqrt(4 * bc * ba)) * RadToDeg;
    }

    public static float WrapAngle(float angle) {
        return angle >= 0f ? Min(angle, 360) : (360f + (angle % 360f));
    }

    public static Vector2 Rotate(Vector2 point, float degrees) {
        //return MathUtilities.RotatePoint(point, degrees);
        float cos = Cos(degrees), sin = Sin(degrees);
        return new Vector2(point.X * cos - point.Y * sin, point.X * sin + point.Y * cos);
    }

    public static Vector2 RotateAround(Vector2 point, Vector2 origin, float degrees) {
        return origin + Rotate(point - origin, degrees);
    }

    #endregion Geometry

    #region Equality

    public static bool ApproxEquals(float a, float b, float tolerance = Epsilon) {
        return Abs(a - b) < tolerance;
    }

    public static bool ApproxEquals(double a, double b, double tolerance = Epsilon) {
        return Abs(a - b) < tolerance;
    }

    public static bool ApproxZero(float a, float tolerance = Epsilon) {
        return ApproxEquals(a, 0.0f, tolerance);
    }

    public static bool ApproxZero(double a, double tolerance = Epsilon) {
        return ApproxEquals(a, 0.0, tolerance);
    }

    #endregion Equality
}
