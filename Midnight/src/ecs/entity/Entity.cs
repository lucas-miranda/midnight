using System.Diagnostics.CodeAnalysis;
using Midnight.Diagnostics;

namespace Midnight;

public struct Entity : System.IEquatable<Entity> {
    public static readonly Entity None = new(0, null);

    public ulong Uid;

    public Entity(ulong uid, Prototype prototype) {
        Uid = uid;
        Prototype = prototype;
    }

    public bool IsDefined => Uid != None.Uid;
    public bool IsUndefined => Uid == None.Uid;
    public Prototype Prototype { get; internal set; }

    public Components GetComponents() {
        return Scene.Current.Components.Get(this);
    }

    public C Add<C>(C component) where C : Component {
        return Scene.Current.Components.Add<C>(this, component);
    }

    public C Add<C>() where C : Component, new() {
        return Scene.Current.Components.Add<C>(this);
    }

    public void AddChild(Entity child) {
        Assert.NotNull(child);
        Transform transform = Get<Transform>();
        Assert.NotNull(transform, "Parent entity doesn't have Transform component.");
        Transform childTransform = child.Get<Transform>();
        Assert.NotNull(childTransform, "Child entity doesn't have Transform component.");
        childTransform.Parent = transform;
    }

    public C Get<C>() where C : Component {
        return Scene.Current.Components.Query<C>(this);
    }

    public bool TryGet<C>(out C component) where C : Component {
        component = Scene.Current.Components.Query<C>(this);
        return component != null;
    }

    public override string ToString() {
        return $"Entity #{Uid}";
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + Uid.GetHashCode();
        }

        return hashCode;
    }

    bool System.IEquatable<Entity>.Equals(Entity entity) {
        return entity.Uid == Uid;
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Entity entity && Equals(entity);
    }

    public static bool operator ==(Entity a, Entity b) {
        return a.Uid == b.Uid;
    }

    public static bool operator !=(Entity a, Entity b) {
        return a.Uid != b.Uid;
    }
}
