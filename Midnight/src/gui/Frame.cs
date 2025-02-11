
namespace Midnight.GUI;

public class Frame : Container {
    public Frame() {
        Background = new() {
            Color = 0x747D8CFF,
            Filled = true,
        };

        Border = new() {
            Color = 0x57606FFF,
            Filled = false,
        };

        Size = new(200, 150);
    }

    public RectangleDrawable Background { get; private set; }
    public RectangleDrawable Border { get; private set; }

    public override void Render(DeltaTime dt) {
        Background.Draw(dt, new DrawParams { Transform = Transform });
        Border.Draw(dt, new DrawParams { Transform = Transform });
        base.Render(dt);
    }

    protected override void SizeChanged() {
        //System.Console.WriteLine($"Frame size defined as: {Size}");
        Background.Size = Border.Size = Size;
    }
}
