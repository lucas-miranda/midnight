
namespace Midnight.GUI;

public class Button : Object {
    public Button() {
        Background = new() {
            Color = 0x57606FFF,
        };

        Text = new() {
            Font = Program.AssetManager.Get<Font>("accidental president"),
            Value = "Button",
        };

        Size = Text.Size;
    }

    public Container Container { get; } = new();
    public bool Pressed { get; private set; }
    public RectangleDrawable Background { get; private set; }
    public StringDrawable Text { get; private set; }

    internal ButtonBuilder Builder { get; set; }

    public override void Render(DeltaTime dt, RenderingServer r) {
        System.Console.WriteLine("Button Draw Begin");
        Background.Draw(dt, r);
        Text.Draw(dt, r);

        foreach (Object obj in Container) {
            obj.Render(dt, r);
        }
        System.Console.WriteLine("Button Draw End");
    }

    public void Press() {
        if (Pressed) {
            return;
        }

        Pressed = true;
        Builder?.RequestEvaluate();
    }

    public void Release() {
        if (!Pressed) {
            return;
        }

        Pressed = false;
        Builder?.RequestEvaluate();
    }

    public override string TreeToString() {
        string str = "[Button |";

        foreach (Object obj in Container) {
            str += $" {obj.TreeToString()}";
        }

        return str + "]";
    }

    protected override void SizeChanged() {
        Background.Size = Text.Size;
    }
}
