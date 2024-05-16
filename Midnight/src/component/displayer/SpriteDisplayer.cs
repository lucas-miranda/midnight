namespace Midnight;

public class SpriteDisplayer : GraphicDisplayer {
    public Texture2D Texture { get; set; }

    public override void Render(DeltaTime dt, RenderingServer r) {
        r.Draw(Texture, Vector2.Zero, Color.White);
    }
}
