
namespace Midnight;

public sealed class UpdateStepEvent : EngineEvent {
    public DeltaTime DeltaTime;

    public UpdateStepEvent(DeltaTime deltaTime) {
        DeltaTime = deltaTime;
    }
}
