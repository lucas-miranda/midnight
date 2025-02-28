
namespace Midnight;

public sealed class RenderStepEvent : EngineEvent {
    public RenderStepEvent(DeltaTime deltaTime) {
        DeltaTime = deltaTime;
    }

    public DeltaTime DeltaTime { get; }
}
