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

    /// <summary>
    /// Prepare anything before Start().
    /// </summary>
    public virtual void Prepare() {
    }

    /// <summary>
    /// Start anything which was prepared at Prepare().
    /// </summary>
    public virtual void Start() {
    }

    public virtual void Update(DeltaTime dt) {
        UpdateComponents(dt);
    }

    public virtual void Render(DeltaTime dt) {
        RenderComponents(dt);
    }

    // TODO: REMOVE ME
    public void ShuffleUpdate() {
        Random.Shuffle(_updateComponents);
    }

    // TODO: REMOVE ME
    public void ShuffleRender() {
        Random.Shuffle(_renderComponents);
    }

    internal void EntityAdded(Entity entity) {
        Logger.Line("Scene > Entity added");
        foreach (Component component in entity.Components) {
            ComponentAdded(component, entity);
        }
    }

    internal void EntityRemoved(Entity entity) {
        Logger.Line("Scene > Entity removed");
        foreach (Component component in entity.Components) {
            ComponentRemoved(component, entity);
        }
    }

    internal void ComponentAdded(Component component, Entity entity) {
        Logger.Line($"Scene > Component '{component.GetType().Name}' added");
        if (component is IUpdatable updatable) {
            Logger.Line("- Updatable");
            Push(updatable, _updateComponents);
        }

        if (component is IRenderable renderable) {
            Logger.Line("- Renderable");
            Push(renderable, _renderComponents);
        }
    }

    internal void ComponentRemoved(Component component, Entity entity) {
        Logger.Line($"Scene > Component '{component.GetType().Name}' removed");
        if (component is IUpdatable updatable) {
            Logger.Line("- Updatable");
            for (int i = 0; i < _updateComponents.Count; i++) {
                ComponentWrapper<IUpdatable> wrapper = _updateComponents[i];

                if (wrapper.Reference == updatable) {
                    Remove(wrapper);
                    break;
                }
            }
        }

        if (component is IRenderable renderable) {
            Logger.Line("- Renderable");
            for (int i = 0; i < _renderComponents.Count; i++) {
                ComponentWrapper<IRenderable> wrapper = _renderComponents[i];

                if (wrapper.Reference == renderable) {
                    Remove(wrapper);
                    break;
                }
            }
        }
    }

    private void UpdateComponents(DeltaTime dt) {
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

    private void RenderComponents(DeltaTime dt) {
        foreach (ComponentWrapper<IRenderable> wrapper in _renderComponents) {
            if (wrapper.Alive) {
                wrapper.Reference.Render(dt);
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
