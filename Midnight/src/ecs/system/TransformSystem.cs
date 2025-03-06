
namespace Midnight;

public class TransformSystem : EntitySystem {
    public override void Setup() {
        Subscribe<UpdateStepEvent>()
            .With<Transform>()
            .Submit(Update);
    }

    private void Update(UpdateStepEvent ev, Query<Transform> transformQuery) {
        if (transformQuery.Entry.Local.FlushMatrix()) {
            transformQuery.Entry.PropagateChanges();
        }
    }
}
