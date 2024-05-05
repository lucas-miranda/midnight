namespace Midnight;

public struct DeltaTime {
    private float _sec;

    public DeltaTime(float sec) {
        _sec = sec;
    }

    public float Sec { get => _sec; }
    public int Milli { get => Time.Sec.ToMilli(Sec); }
}
