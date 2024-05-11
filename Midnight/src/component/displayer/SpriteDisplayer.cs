namespace Midnight;

public class SpriteDisplayer : GraphicDisplayer {
    public Texture2D Texture { get; set; }

    public override void Render() {
        Game.Rendering.Draw(Texture);
    }
}
