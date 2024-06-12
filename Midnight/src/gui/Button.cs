
namespace Midnight.GUI;

public class Button : Object {
    public Container Container { get; } = new();
    public bool Pressed { get; private set; }

    internal ButtonBuilder Builder { get; set; }

    public override void Render(DeltaTime dt, RenderingServer r) {
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
    }
}
