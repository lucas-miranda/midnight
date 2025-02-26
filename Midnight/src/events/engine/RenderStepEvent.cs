
namespace Midnight;

public sealed class RenderStepEvent : EngineEvent {
    public DeltaTime DeltaTime;

    public RenderStepEvent(DeltaTime deltaTime) {
        DeltaTime = deltaTime;
    }
}
