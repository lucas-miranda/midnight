
namespace Midnight;

public sealed class UpdateStepEvent : EngineEvent {
    public UpdateStepEvent(DeltaTime deltaTime) {
        DeltaTime = deltaTime;
    }

    public DeltaTime DeltaTime { get; }
}
