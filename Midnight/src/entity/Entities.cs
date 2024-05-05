using System.Collections.Generic;

namespace Midnight;

public class Entities {
    private ulong NextUid;
    private List<Entity> _entries = new();

    public Entities(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public void Register(Entity entity) {
        _entries.Add(entity);
        entity.Uid = NextUid;
        NextUid += 1;
        EntityRegistered(entity);
    }

    public void Shuffle() {
        Random.Shuffle(_entries);
    }

    private void EntityRegistered(Entity entity) {
        Scene.EntityAdded(entity);
        entity.SceneAdded(Scene);
    }

    private void EntityUnregistered(Entity entity) {
        // TODO  unused
    }
}
