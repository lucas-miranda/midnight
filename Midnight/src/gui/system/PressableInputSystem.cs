namespace Midnight.GUI;

public sealed class PressableInputSystem : EntitySystem {
    public override void Setup() {
        Subscribe<MouseButtonEvent>()
            .With<Pressable>()
            .With<Transform>()
            .With<Extent>()
            .Submit(HandleMouseButton);
    }

    public void HandleMouseButton(MouseButtonEvent e, Query<Pressable> pressable, Query<Transform> transform, Query<Extent> extent) {
        Logger.DebugLine("PressableInputSystem -> HandleMouseButton");

        switch (e) {
            case MouseButtonEvent mouseButtonEvent:
                //Logger.DebugLine($"----");
                //Logger.DebugLine($"Button Event: {mouseButtonEvent}");
                //Logger.DebugLine($"Button Bounds: {Bounds}");

                if (mouseButtonEvent.Button == MouseButton.Left
                 && extent.Entry.GetBounds(transform.Entry.Global).IsInside(mouseButtonEvent.Position)
                ) {
                    switch (mouseButtonEvent.State) {
                        case ButtonState.JustPressed:
                            pressable.Entry.Pressed = true;
                            break;
                        case ButtonState.JustReleased:
                            pressable.Entry.Pressed = false;
                            break;
                        default:
                            break;
                    }
                }

                break;
            default:
                break;
        }
    }
}
