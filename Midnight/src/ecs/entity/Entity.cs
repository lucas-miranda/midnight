using System.Diagnostics.CodeAnalysis;
using Midnight.Diagnostics;

namespace Midnight;

public struct Entity : System.IEquatable<Entity> {
    public static readonly Entity None = new();

    public ulong Uid;

    public Entity() : this(0, null) {
    }

    public Entity(ulong uid, Prototype prototype, Components components = null) {
        Uid = uid;
        Prototype = prototype;
        Components = components != null ? components : new();
        Assert.NotNull(Components);
        Components.Entity = this;
    }

    public bool IsDefined => Uid != None.Uid;
    public bool IsUndefined => Uid == None.Uid;
    public Prototype Prototype { get; }
    public Components Components { get; }

    public C Add<C>(C component) where C : Component {
        return Components.Add<C>(component);
    }

    public C Add<C>() where C : Component, new() {
        return Components.Add<C>(new());
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
        return Components.Get<C>();
    }

    public (C1, C2) Get<C1, C2>()
        where C1 : Component
        where C2 : Component
    {
        return (Components.Get<C1>(), Components.Get<C2>());
    }

    public (C1, C2, C3) Get<C1, C2, C3>()
        where C1 : Component
        where C2 : Component
        where C3 : Component
    {
        return (Components.Get<C1>(), Components.Get<C2>(), Components.Get<C3>());
    }

    public bool TryGet<C>(out C c) where C : Component {
        return Components.TryGet<C>(out c);
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
