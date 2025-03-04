
namespace Midnight.GUI;

public class ResizeEvent : UIEvent {
    public ResizeEvent(Size2 size) {
        Size = size;
    }

    public Size2 Size { get; }
}
