
namespace Midnight;

public class DrawableDisplayer : GraphicDisplayer {
    public DrawableDisplayer() {
    }

    public Drawable Drawable { get; set; }

    public override void Update(DeltaTime dt) {
    }

    public override void Render(DeltaTime dt) {
        Drawable?.Draw(dt);
    }
}
