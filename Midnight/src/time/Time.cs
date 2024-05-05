
namespace Midnight;

public static class Time {
    public static class Sec {
        public const int AsMilli = 1000;

        public static int ToMilli(float sec) {
            return Math.Floori(sec * AsMilli);
        }
    }

    public static class Milli {
        public const float AsSec = 1.0f / 1000.0f;

        public static float ToSec(int milli) {
            return milli * AsSec;
        }
    }
}
