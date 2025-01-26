using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct DeltaTime {
    private float _sec;

    public DeltaTime(float sec) {
        _sec = sec;
    }

    internal DeltaTime(Xna.GameTime gameTime) {
        _sec = (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    public float Sec => _sec;
    public int Milli => Time.Sec.ToMilli(Sec);

    public static implicit operator float(DeltaTime dt) {
        return dt.Sec;
    }

    public static implicit operator int(DeltaTime dt) {
        return dt.Milli;
    }

    public static implicit operator uint(DeltaTime dt) {
        return (uint) dt.Milli;
    }
}
