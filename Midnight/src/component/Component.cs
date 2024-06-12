
namespace Midnight;

public abstract class Component {
    public Entity Entity { get; private set; }

    public virtual void EntityAdded(Entity entity) {
        Entity = entity;
    }

    public virtual void EntityRemoved(Entity entity) {
        Entity = null;
    }
}
