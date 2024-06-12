
namespace Midnight;

public sealed class Entity {
    public Entity() {
        Components = new(this);
    }

    public ulong Uid { get; internal set; }
    public Scene Scene { get; private set; }
    public Components Components { get; }

    public void SceneAdded(Scene scene) {
        Scene = scene;
    }

    public void SceneRemoved() {
        Scene = null;
    }

    public void ComponentAdded(Component component) {
        Scene?.ComponentAdded(component, this);
        component.EntityAdded(this);
    }

    public void ComponentRemoved(Component component) {
        Scene?.ComponentRemoved(component, this);
        component.EntityRemoved(this);
    }
}
