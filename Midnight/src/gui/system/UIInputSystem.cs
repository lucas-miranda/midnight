namespace Midnight.GUI;

public sealed class UIInputSystem : EntitySystem {
    public override void Setup() {
        //Subscribe<MouseButtonEvent, GUIDisplayer>(HandleMouseButton);
        Subscribe<MouseButtonEvent>()
            .With<Pressable>()
            .With<Transform>()
            .With<Extent>()
            .Submit(HandleMouseButton);
    }

    public void HandleMouseButton(MouseButtonEvent e, Query<Pressable> pressable, Query<Transform> transform, Query<Extent> extent) {
        Rectangle bounds = extent.Entry.GetBounds(transform.Entry.Global);

        if (bounds.Contains(e.Position)) {
            switch (e.Button) {
                case MouseButton.Left:
                    switch (e.State) {
                        case ButtonState.JustPressed:
                            pressable.Entry.Pressed = true;
                            break;
                        case ButtonState.JustReleased:
                            pressable.Entry.Pressed = false;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /*
    public void HandleMouseButton(MouseButtonEvent e, GUIDisplayer displayer) {
        Logger.DebugLine("GUISystem -> HandleMouseButton");
        //displayer.Design.Root.Input(e);
    }
    */
}
