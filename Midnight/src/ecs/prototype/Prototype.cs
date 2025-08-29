namespace Midnight;

public abstract class Prototype {
    public Prototype() {
    }

    public EntityBuilder Builder { get; private set; }

    public Entity Instantiate() {
        Builder = Scene.Current.Entities.Create();
        Build();
        Entity e = Builder.Submit(this);
        Builder = null;
        return e;
    }

    protected abstract void Build();

    protected C Add<C>() where C : Component, new() {
        return Builder.Add<C>();
    }

    protected C Add<C>(C component) where C : Component, new() {
        return Builder.Add(component);
    }

    protected EntityBuilder With<C>() where C : Component, new() {
        return Builder.With<C>();
    }

    protected EntityBuilder With<C>(C component) where C : Component {
        return Builder.With(component);
    }

    protected EntityBuilder With<C>(out C component) where C : Component, new() {
        return Builder.With<C>(out component);
    }
}
