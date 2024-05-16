using System.Collections.Generic;

namespace Midnight;

public class Scene {
    private List<ComponentWrapper<IUpdatable>> _updateComponents = new(100);
    private List<ComponentWrapper<IRenderable>> _renderComponents = new(100);
    //private Dictionary<System.Type, List<Component>> _components = new(50);

    public Scene() {
        Entities = new(this);
    }

    public Entities Entities { get; }

    public virtual void Prepare() {
    }

    public virtual void Start() {
    }

    public void ShuffleUpdate() {
        Random.Shuffle(_updateComponents);
    }

    public void ShuffleRender() {
        Random.Shuffle(_renderComponents);
    }

    internal void Update(DeltaTime dt) {
        for (int i = 0; i < _updateComponents.Count; i++) {
            ComponentWrapper<IUpdatable> wrapper = _updateComponents[i];

            if (!wrapper.Alive) {
                _updateComponents.RemoveAt(i);
                i -= 1;
                continue;
            }

            wrapper.Reference.Update(dt);
        }

        for (int i = 0; i < _renderComponents.Count; i++) {
            ComponentWrapper<IRenderable> wrapper = _renderComponents[i];

            if (!wrapper.Alive) {
                _renderComponents.RemoveAt(i);
                i -= 1;
            }
        }
    }

    internal void Render(DeltaTime dt, RenderingServer r) {
        foreach (ComponentWrapper<IRenderable> wrapper in _renderComponents) {
            if (wrapper.Alive) {
                wrapper.Reference.Render(dt, r);
            }
        }
    }

    internal void EntityAdded(Entity entity) {
        System.Console.WriteLine("Scene > Entity added");
        foreach (Component component in entity.Components) {
            ComponentAdded(component, entity);
        }
    }

    internal void EntityRemoved(Entity entity) {
        System.Console.WriteLine("Scene > Entity added");
        foreach (Component component in entity.Components) {
            ComponentRemoved(component, entity);
        }
    }

    internal void ComponentAdded(Component component, Entity entity) {
        if (component is IUpdatable updatable) {
            Push(updatable, _updateComponents);
        } else if (component is IRenderable renderable) {
            Push(renderable, _renderComponents);
        }
    }

    internal void ComponentRemoved(Component component, Entity entity) {
        if (component is IUpdatable updatable) {
            for (int i = 0; i < _updateComponents.Count; i++) {
                ComponentWrapper<IUpdatable> wrapper = _updateComponents[i];

                if (wrapper.Reference == updatable) {
                    Remove(wrapper);
                    break;
                }
            }
        } else if (component is IRenderable renderable) {
            for (int i = 0; i < _renderComponents.Count; i++) {
                ComponentWrapper<IRenderable> wrapper = _renderComponents[i];

                if (wrapper.Reference == renderable) {
                    Remove(wrapper);
                    break;
                }
            }
        }
    }

    private void Push<T>(T component, List<ComponentWrapper<T>> target) {
        target.Add(new(component));
    }

    private bool Remove<T>(ComponentWrapper<T> wrapper) {
        bool removed = wrapper.Alive;
        wrapper.Alive = false;
        return removed;
    }


    private struct ComponentWrapper<T> {
        public T Reference;
        public bool Alive;

        public ComponentWrapper(T reference) {
            Reference = reference;
            Alive = true;
        }
    }
}
