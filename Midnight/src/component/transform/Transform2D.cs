using System.Collections;
using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

/// <summary>
/// Describes a 2D transformation.
/// The sequence of operations are: Scale, Rotation and Translation.
/// </summary>
public class Transform2D : Component, IEnumerable<Transform2D> {
    private Vector2 _position,
                    _scale = Vector2.One,
                    _scaleOrigin,
                    _rotationOrigin;

    private float _rotation,
                  _scaleRotation;

    private bool _hasChanges = true;
    private Transform2D _parent;
    private List<Transform2D> _children = new();
    private ITransformObject _owner;

    /// <summary>
    /// Local position.
    /// It's the third and last operation applied.
    /// </summary>
    public Vector2 Position {
        get => _position;
        set {
            _position = value;
            _hasChanges = true;
        }
    }

    /// <summary>
    /// Local scale.
    /// It's the first operation applied.
    /// </summary>
    public Vector2 Scale {
        get => _scale;
        set {
            _scale = value;
            _hasChanges = true;
        }
    }

    /// <summary>
    /// Local rotation.
    /// It's the second operation applied.
    /// </summary>
    public float Rotation {
        get => _rotation;
        set {
            _rotation = value;
            _hasChanges = true;
        }
    }

    public Vector2 GlobalPosition {
        get => Position + (Parent?.GlobalPosition ?? Vector2.Zero);
        set => Position = value - (Parent?.GlobalPosition ?? Vector2.Zero);
    }

    public Vector2 GlobalScale {
        get => Scale * (Parent?.GlobalScale ?? Vector2.One);
        set => Scale = value / (Parent?.GlobalScale ?? Vector2.One);
    }

    public float GlobalRotation {
        get => Rotation + (Parent?.GlobalRotation ?? 0.0f);
        set => Rotation = value - (Parent?.GlobalRotation ?? 0.0f);
    }

    /// <summary>
    /// Resulting matrix.
    /// By multiplying it by a vector, applies the stored transformation.
    /// </summary>
    /// <see cref="Apply(Vector3)"/>
    public Matrix Matrix {
        get {
            if (Parent == null) {
                return LocalMatrix;
            }

            return Parent.Matrix * LocalMatrix;
        }
    }

    /// <summary>
    /// Resulting local matrix.
    /// By multiplying it by a vector, applies the stored transformation.
    /// </summary>
    /// <see cref="Apply(Vector3)"/>
    public Matrix LocalMatrix { get; set; }

    /// <summary>
    /// Origin which should be used when applying Scale.
    /// Origin is a point related to model space.
    /// </summary>
    public Vector2 ScaleOrigin {
        get => _scaleOrigin;
        set {
            _scaleOrigin = value;
            _hasChanges = true;
        }
    }

    /// <summary>
    /// Origin which should be used when applying Rotation.
    /// Origin is a point related to model space.
    /// </summary>
    public Vector2 RotationOrigin {
        get => _rotationOrigin;
        set {
            _rotationOrigin = value;
            _hasChanges = true;
        }
    }

    /// <summary>
    /// Origin which should be used when applying Scale and Rotation.
    /// Origin is a point related to model space.
    /// THis value is always the same as <see cref="ScaleOrigin"/>.
    /// </summary>
    public Vector2 Origin {
        get => ScaleOrigin;
        set => ScaleOrigin = RotationOrigin = value;
    }

    /// <summary>
    /// Rotation of the Scale operation.
    /// This rotation transforms the scale vector itself, before it being finally applied.
    /// </summary>
    public float ScaleRotation {
        get => _scaleRotation;
        set {
            _scaleRotation = value;
            _hasChanges = true;
        }
    }

    public ITransformObject Owner {
        get => _owner;
        set {
            Assert.Null(_owner);
            _owner = value;
            OwnerChanged(_owner);
        }
    }

    public Transform2D Parent {
        get => _parent;
        set {
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

    public int ChildCount => _children.Count;

    public override void EntityAdded(Entity entity) {
        base.EntityAdded(entity);
    }

    public override void EntityRemoved(Entity entity) {
        base.EntityRemoved(entity);
    }

    public void OwnerChanged(ITransformObject owner) {
    }

    public void FlushMatrix() {
        if (!_hasChanges) {
            return;
        }

        _hasChanges = false;

        Matrix mso = Matrix.Translation(_scaleOrigin),
               msr = Matrix.Rotation(_scaleRotation),
               ms = Matrix.Scaling(_scale),
               mro = Matrix.Translation(_rotationOrigin),
               mr = Matrix.Rotation(_rotation),
               mt = Matrix.Translation(_position);

        LocalMatrix = mt * mro * mr * mro.Invert() * mso * msr * ms * msr.Invert() * mso.Invert();
    }

    /// <summary>
    /// Applies transformation to a Vector3.
    /// </summary>
    public Vector3 Apply(Vector3 v) {
        return Matrix * v;
    }

    public IEnumerator<Transform2D> GetEnumerator() {
        return _children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public override string ToString() {
        return $"Pos: {Position} (G: {GlobalPosition}); Scale: {Scale} (G: {GlobalScale}; Origin: {ScaleOrigin}; Rot: {ScaleRotation}); Rot: {Rotation} (G: {GlobalRotation}; Origin: {RotationOrigin});";
    }

    private void AddChild(Transform2D child) {
        _children.Add(child);
        child._parent = this;
        child.ReceiveParent();
        ChildAdded(child);
    }

    private void RemoveChild(Transform2D child) {
        Assert.True(child._parent == this);
        _children.Remove(child);
        child._parent = null;
        child.LostParent(this);
        ChildRemoved(child);
    }

    private void ReceiveParent() {
    }

    private void LostParent(Transform2D parent) {
    }

    private void ChildAdded(Transform2D child) {
    }

    private void ChildRemoved(Transform2D child) {
    }
}
