namespace Midnight;

/// <summary>
/// Describes a 2D transformation.
/// The sequence of operations are: Scale, Rotation and Translation.
/// </summary>
public class Transform2D {
    public static readonly Transform2D Identity = new();

    private Matrix _matrix = Matrix.Identity;
    private Vector2 _position,
                    _scale = Vector2.One,
                    _scaleOrigin,
                    _rotationOrigin;

    private float _rotation,
                  _scaleRotation;

    private bool _hasChanges = true;

    public Transform2D() {
    }

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
    public Matrix Matrix {
        get => _matrix;
        set {
            _hasChanges = false;
            _matrix = value;
            _scaleOrigin = _rotationOrigin = Vector2.Zero;
            _scaleRotation = 0.0f;
            _matrix.Decompose(out _position, out _scale, out _rotation);
        }
    }

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

    public bool FlushMatrix() {
        if (!_hasChanges) {
            return false;
        }

        _hasChanges = false;

        Matrix mso = Matrix.Translation(_scaleOrigin),
               msr = Matrix.RotationZ(_scaleRotation),
               ms = Matrix.Scaling(_scale),
               mro = Matrix.Translation(_rotationOrigin),
               mr = Matrix.RotationZ(_rotation),
               mt = Matrix.Translation(_position);

        _matrix = mso.Invert() * msr.Invert() * ms * msr * mso * mro.Invert() * mr * mro * mt;
        return true;
    }

    /// <summary>
    /// Applies transformation to a Vector3.
    /// </summary>
    public Vector3 Apply(Vector3 v) {
        return v * Matrix;
    }

    public void CopyFrom(Transform2D transform) {
        _position = transform.Position;
        _scale = transform.Scale;
        _scaleOrigin = transform.ScaleOrigin;
        _rotationOrigin = transform.RotationOrigin;
        _rotation = transform.Rotation;
        _scaleRotation = transform.ScaleRotation;
        _hasChanges = transform._hasChanges;
        _matrix = transform.Matrix;
    }

    public void Push(Transform2D transform) {
        FlushMatrix();
        transform.FlushMatrix();
        _scaleOrigin = _rotationOrigin = Vector2.Zero;
        _scaleRotation = 0.0f;
        _matrix *= transform.Matrix;
        _matrix.Decompose(out _position, out _scale, out _rotation);
    }

    public override string ToString() {
        return $"Pos: {Position}; Scale: {Scale} (Origin: {ScaleOrigin}; Rot: {ScaleRotation}); Rot: {Rotation} (Origin: {RotationOrigin});";
    }
}
