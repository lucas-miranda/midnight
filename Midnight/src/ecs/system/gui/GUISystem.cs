namespace Midnight;

public sealed class GUISystem : EntitySystem {
    public override void Setup() {
        Subscribe<MouseButtonEvent, GUIDisplayer>(HandleMouseButton);
    }

    public void HandleMouseButton(MouseButtonEvent e, GUIDisplayer displayer) {
        Logger.DebugLine("GUISystem -> HandleMouseButton");
        displayer.Design.Root.Input(e);
    }
}
