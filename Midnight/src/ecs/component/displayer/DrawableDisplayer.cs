
namespace Midnight;

public class DrawableDisplayer : GraphicDisplayer {
    public DrawableDisplayer() {
    }

    public Drawable Drawable { get; set; }
    public override Size2 Size => Drawable.Size;
}
