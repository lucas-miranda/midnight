
namespace Midnight;

public sealed class FPS {
    private float _elapsedTime;

    /// <summary>
    /// Current Frames Per Second.
    /// </summary>
    public int Current { get; private set; }

    /// <summary>
    /// How many frames are rendered until now.
    /// Since the last second.
    /// </summary>
    /// <remarks>
    /// Frame count are accumulated here to later be used to calculate how many frames are per second.
    /// </remarks>
    public int FramesRendered { get; private set; }

    public void Update(DeltaTime deltaTime) {
        _elapsedTime += deltaTime;

        if (_elapsedTime >= 1.0f - Math.Epsilon) {
            _elapsedTime -= 1.0f;
            Current = FramesRendered;
            FramesRendered = 0;
        }
    }

    public void PreFrameRendered() {
        FramesRendered += 1;
    }
}
