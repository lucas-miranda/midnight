namespace Midnight.GUI;

public sealed class PressableInputSystem : EntitySystem {
    public override void Setup() {
        Subscribe<MouseButtonEvent, Pressable, Transform, Extent>(HandleMouseButton);
    }

    public void HandleMouseButton(MouseButtonEvent e, Pressable pressable, Transform transform, Extent extent) {
        Logger.DebugLine("PressableInputSystem -> HandleMouseButton");

        switch (e) {
            case MouseButtonEvent mouseButtonEvent:
                //Logger.DebugLine($"----");
                //Logger.DebugLine($"Button Event: {mouseButtonEvent}");
                //Logger.DebugLine($"Button Bounds: {Bounds}");

                if (mouseButtonEvent.Button == MouseButton.Left
                 && extent.GetBounds(transform.Global).IsInside(mouseButtonEvent.Position)
                ) {
                    switch (mouseButtonEvent.State) {
                        case ButtonState.JustPressed:
                            pressable.Pressed = true;
                            break;
                        case ButtonState.JustReleased:
                            pressable.Pressed = false;
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
