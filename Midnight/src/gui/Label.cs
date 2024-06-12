
namespace Midnight.GUI;

public class Label : Object {
    public string Text { get; set; }

    public override void Render(DeltaTime dt, RenderingServer r) {
    }

    public override string TreeToString() {
        return $"[Label '{Text}']";
    }

    protected override void SizeChanged() {
    }
}
