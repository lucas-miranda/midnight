using Midnight.Diagnostics;

namespace Midnight;

public class Camera {
    public Matrix World = Matrix.Identity,
                  View = Matrix.Identity,
                  Projection = Matrix.Identity,
                  WorldViewProjection = Matrix.Identity;

    private bool _requireRecalculateView,
                 _requireRecalculateProjection;

    private Matrix _viewProjection = Matrix.Identity;
    private Vector3 _position;
    private Size2I _size;
    private ProjectionKind _projectionKind;

    public Camera() {
        BackBuffer backbuffer = GraphicsServer.BackBuffer;
        _size = backbuffer.Size;
        RequestRecalculate();
    }

    public Vector3 Position {
        get => _position;
        set {
            _position = value;
            RequestRecalculateView();
        }
    }

    public Size2I Size {
        get => _size;
        set {
            Assert.True(value.IsEmpty(), "Size can't be empty.");
            _size = value;
            RequestRecalculateProjection();
        }
    }

    public Vector2 Center {
        get => Position.ToVec2() + Size / 2.0f;
        set => Position = new(value - Size / 2.0f, Position.Z);
    }

    public ProjectionKind ProjectionKind {
        get => _projectionKind;
        set {
            if (value == _projectionKind) {
                return;
            }

            _projectionKind = value;
            RequestRecalculateProjection();
        }
    }

    public void FocusOrigin() {
        Center = Vector2.Zero;
    }

    public void Recalculate() {
        if (!(_requireRecalculateView || _requireRecalculateProjection)) {
            return;
        }

        if (_requireRecalculateProjection) {
            switch (ProjectionKind) {
                case ProjectionKind.Orthographic:
                    if (Size.ApproxZero()) {
                        break;
                    }

                    // TODO  add custom config to near/far planes
                    //World = Matrix.Translation(-Size.ToVector2() / 2.0f, 0.0f);
                    //Projection = Matrix.OrthoRH(Size.Width, Size.Height, -100, 100);
                    Projection = Matrix.OrthoOffCenterRH(Size.Height, 0.0f, 0.0f, Size.Width, 0.0f, 1.0f);
                    break;

                case ProjectionKind.Perspective:
                    throw new System.NotSupportedException();

                default:
                    throw new System.NotSupportedException();
            }

            _requireRecalculateProjection = false;
        }

        if (_requireRecalculateView) {
            // TODO  add custom config to target and up
            View = Matrix.LookAtRH(Position, Position + new Vector3(0.0f, 0.0f, -1.0f), new(0.0f, 1.0f, 0.0f));

            _requireRecalculateView = false;
        }

        _viewProjection = Matrix.Multiply(ref View, ref Projection);
        WorldViewProjection = Matrix.Multiply(ref World, ref _viewProjection);
    }

    public void RequestRecalculate() {
        _requireRecalculateView =
            _requireRecalculateProjection = true;
    }

    public void RequestRecalculateView() {
        _requireRecalculateView = true;
    }

    public void RequestRecalculateProjection() {
        _requireRecalculateProjection = true;
    }
}
