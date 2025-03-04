namespace Midnight.GUI;

public class Extent : Component {
    private Size2 _size;

    public Size2 Size {
        get => _size;
        set {
            if (value == _size) {
                return;
            }

            _size = value;
            //RequestLayout();
            //SizeChanged();
        }
    }

    public Spacing Margin { get; set; }
    public Spacing Padding { get; set; }

    public Rectangle GetBounds(Transform2D transform) {
        return new(transform.Position, Size);
    }
}
