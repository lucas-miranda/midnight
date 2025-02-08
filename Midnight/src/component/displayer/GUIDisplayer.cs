namespace Midnight;

public class GUIDisplayer : GraphicDisplayer {
    public GUI.Design Design { get; } = new();

    public override void Update(DeltaTime dt) {
        if (Design.Root == null) {
            return;
        }

        Design.Root.Update(dt);
    }

    public override void Render(DeltaTime dt, RenderingServer r) {
        // TODO  draws GUI.Design here
        if (Design.Root == null) {
            return;
        }

        Design.Root.Render(dt, r);
    }
}
