
namespace Midnight.GUI;

public class Frame : Object, IContainer {
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

    public Container Container { get; } = new();
    public RectangleDrawable Background { get; private set; }
    public RectangleDrawable Border { get; private set; }

    public override void Render(DeltaTime dt, RenderingServer r) {
        System.Console.WriteLine("Frame Draw Begin");
        Background.Draw(dt, r);
        Border.Draw(dt, r);

        foreach (Object obj in Container) {
            obj.Render(dt, r);
        }
        System.Console.WriteLine("Frame Draw End");
    }

    public override string TreeToString() {
        string str = $"[Frame Bg: {Background.Color}, Border: {Border.Color}, bg l: {Background._layer}, size: {Size} |";

        foreach (Object obj in Container) {
            str += $" {obj.TreeToString()};";
        }

        return str + "]";
    }

    protected override void SizeChanged() {
        Background.Size = Border.Size = Size;
    }
}
