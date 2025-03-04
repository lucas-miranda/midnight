
namespace Midnight;

public class TransformSystem : EntitySystem {
    public override void Setup() {
        Subscribe<UpdateStepEvent, Transform>(Update);
    }

    private void Update(UpdateStepEvent ev, Transform transform) {
        if (transform.Local.FlushMatrix()) {
            transform.PropagateChanges();
        }
    }
}
