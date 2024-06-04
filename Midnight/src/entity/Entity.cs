
namespace Midnight;

public class Entity {
    public Entity() {
        Components = new(this);
    }

    public ulong Uid { get; internal set; }
    public Scene Scene { get; private set; }
    public Components Components { get; }

    public virtual void SceneAdded(Scene scene) {
        Scene = scene;
    }

    public virtual void SceneRemoved() {
        Scene = null;
    }

    public virtual void ComponentAdded(Component component) {
        Scene?.ComponentAdded(component, this);
        component.EntityAdded(this);
    }

    public virtual void ComponentRemoved(Component component) {
        Scene?.ComponentRemoved(component, this);
        component.EntityRemoved(this);
    }
}
