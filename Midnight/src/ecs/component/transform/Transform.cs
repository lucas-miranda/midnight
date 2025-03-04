using System.Collections.Generic;
using System.Collections;
using Midnight.Diagnostics;

namespace Midnight;

public class Transform : Component, IEnumerable<Transform> {
    private List<Transform> _children = new();
    private Transform _parent;

    public Transform() {
        Local = new();
        Global = new();
    }

    public Transform2D Local { get; private set; }
    public Transform2D Global { get; private set; }
    public int ChildCount => _children.Count;

    public Transform Parent {
        get => _parent;
        set {
            if (value == this) {
                throw new System.InvalidOperationException("Transform can't be parent of itself.");
            }

            if (value != null) {
                if (value == _parent) {
                    // value already is current parent
                    return;
                } else if (_parent != null) {
                    _parent.RemoveChild(this);
                }

                value.AddChild(this);
            } else if (_parent != null) {
                _parent.RemoveChild(this);
            }
        }
    }

    public bool HasParent => Parent != null;

    public C FindFirstChildWithComponent<C>() where C : Component {
        foreach (Transform childTransform in this) {
            if (childTransform.Entity.TryGet<C>(out C childComponent)) {
                return childComponent;
            }
        }

        return null;
    }

    public void PropagateChanges() {
        if (HasParent) {
            Global.CopyFrom(Parent.Global);
        } else {
            Global.CopyFrom(Transform2D.Identity);
        }

        Global.Push(Local);

        foreach (Transform child in this) {
            child.PropagateChanges();
        }
    }

    public IEnumerator<Transform> GetEnumerator() {
        return _children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    private void AddChild(Transform child) {
        _children.Add(child);
        child._parent = this;
        child.ReceiveParent();
        ChildAdded(child);
    }

    private void RemoveChild(Transform child) {
        Assert.True(child._parent == this);
        _children.Remove(child);
        child._parent = null;
        child.LostParent(this);
        ChildRemoved(child);
    }

    private void ReceiveParent() {
    }

    private void LostParent(Transform parent) {
    }

    private void ChildAdded(Transform child) {
    }

    private void ChildRemoved(Transform child) {
    }
}
