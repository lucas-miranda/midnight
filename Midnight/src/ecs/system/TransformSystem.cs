
namespace Midnight;

[SystemRegistry]
public class TransformSystem : EntitySystem {
    public override void Setup(Scene scene) {
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
