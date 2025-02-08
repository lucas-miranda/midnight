
namespace Midnight.GUI;

public class Label : Object {
    public Label() {
        Text = new() {
            Font = Program.AssetManager.Get<Font>("accidental president"),
        };

        Size = Text.Size;
    }

    public string Value {
        get => Text.Value;
        set {
            Text.Value = value;
            Size = Text.Size;
        }
    }

    public StringDrawable Text { get; private set; }

    public override void Render(DeltaTime dt, RenderingServer r) {
        Text.Draw(dt, r, new DrawParams { Transform = Transform });
    }

    public override string TreeToString() {
        return $"[Label '{Value}']";
    }

    protected override void Layout() {
    }

    protected override void SizeChanged() {
    }
}
