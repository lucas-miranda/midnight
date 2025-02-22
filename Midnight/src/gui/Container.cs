using System.Collections.Generic;
using System.Collections;
using Midnight.Diagnostics;

namespace Midnight.GUI;

public abstract class Container : Object, IEnumerable<Object> {
    public Container() {
    }

    public int ChildCount => Transform.ChildCount;
    public ContainerLayout Layout { get; set; } = new HorizontalBoxLayout();

    public override void Render(DeltaTime dt) {
        base.Render(dt);

        foreach (Object child in this) {
            child.Render(dt);
        }
    }

    public void Add(Object obj) {
        Assert.NotNull(obj);
        obj.Transform.Parent = Transform;
        RequestLayout();
    }

    public bool Remove(Object obj) {
        Assert.NotNull(obj);

        if (obj.Transform.Parent != Transform) {
            return false;
        }

        obj.Transform.Parent = null;
        RequestLayout();
        return true;
    }

    public bool IsEmpty() {
        return ChildCount <= 0;
    }

    public T Find<T>(bool recursive = true) where T : Object {
        foreach (Transform2D child in Transform) {
            if (child.Owner is T t) {
                return t;
            } else if (recursive && child.Owner is Container subContainer) {
                T result = subContainer.Find<T>();
                if (result != null) {
                    return result;
                }
            }
        }

        return null;
    }

    public IEnumerator<Object> GetEnumerator() {
        foreach (Transform2D child in Transform) {
            if (child.Owner is Object obj) {
                yield return obj;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public override string TreeToString() {
        string str = $"[{GetType().Name} |";

        foreach (Object child in this) {
            str += $" {child.TreeToString()};";
        }

        return str + "]";
    }

    protected override void ExecuteLayout() {
        Layout?.Execute(this);
    }

    private Rectangle? CalculateChildrenBounds() {
        if (IsEmpty()) {
            return null;
        }

        Rectangle? bounds = null;

        foreach (Object child in this) {
            if (!bounds.HasValue) {
                bounds = child.Bounds;
            } else {
                bounds = Rectangle.Enclose(bounds.Value, child.Bounds);
            }
        }

        return bounds;
    }
}
