
namespace Midnight;

public sealed class Entity {
    public Entity() {
        Components = new(this);
    }

    public ulong Uid { get; internal set; }
    public Scene Scene { get; private set; }
    public Components Components { get; }

    public static Entity Create() {
        return new();
    }

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

    public Entity With<T>() where T : Component, new() {
        Components.Create<T>();
        return this;
    }

    public Entity With<T>(out T component) where T : Component, new() {
        component = Components.Create<T>();
        return this;
    }

    public Entity With<T>(T component) where T : Component {
        Components.Add(component);
        return this;
    }

    public T Get<T>() where T : Component {
        return Components.Get<T>();
    }
}
