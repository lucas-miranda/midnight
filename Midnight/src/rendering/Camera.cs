using Midnight.Diagnostics;

namespace Midnight;

public class Camera {
    public Matrix View = Matrix.Identity,
                  Projection = Matrix.Identity,
                  ViewProjection = Matrix.Identity;

    private bool _requireRecalculateView,
                 _requireRecalculateProjection;

    private Vector3 _position;
    private Size2I _size;
    private ProjectionKind _projectionKind;

    public Camera() {
        BackBuffer backbuffer = Program.Graphics.BackBuffer;
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
                    Projection = Matrix.Ortho(Size.Width, Size.Height, -100, 100);
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
            View = Matrix.LookAt(Position, Position + new Vector3(0.0f, 0.0f, 1.0f), new(0.0f, -1.0f, 0.0f));
            _requireRecalculateView = false;
        }

        ViewProjection = Matrix.Multiply(ref View, ref Projection);
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
