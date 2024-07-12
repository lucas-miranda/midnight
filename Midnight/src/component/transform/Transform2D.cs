
namespace Midnight;

/// <summary>
/// Describes a 2D transformation.
/// The sequence of operations are: Scale, Rotation and Translation.
/// </summary>
public class Transform2D : Component {
    private Vector2 _position,
                    _scale = Vector2.One,
                    _scaleOrigin,
                    _rotationOrigin;

    private float _rotation, _scaleRotation;
    private bool _hasChanges = true;
    private Matrix _matrix;

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

    /// <summary>
    /// Resulting matrix.
    /// By multiplying it by a vector, applies the stored transformation.
    /// </summary>
    /// <see cref="Apply(Vector3)"/>
    public Matrix Matrix { get; set; }

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

    public override void EntityAdded(Entity entity) {
        base.EntityAdded(entity);
    }

    public override void EntityRemoved(Entity entity) {
        base.EntityRemoved(entity);
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

        Matrix = mt * mro * mr * mro.Invert() * mso * msr * ms * msr.Invert() * mso.Invert();
    }

    /// <summary>
    /// Applies transformation to a Vector3.
    /// </summary>
    public Vector3 Apply(Vector3 v) {
        return Matrix * v;
    }

    public override string ToString() {
        return $"Pos: {Position}; Scale: {Scale} (Origin: {ScaleOrigin}; Rot: {ScaleRotation}); Rot: {Rotation} (Origin: {RotationOrigin});";
    }
}
