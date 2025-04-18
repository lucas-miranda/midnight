namespace Midnight.GUI;

[SystemRegistry]
public sealed class PressableInputSystem : EntitySystem {
    public override void Setup(Scene scene) {
        Subscribe<MouseButtonEvent>()
            .With<Pressable>()
            .With<Transform>()
            .With<Extent>()
            .With<Widget>()
            .Submit(HandleMouseButton);

        Subscribe<PressableInteractEvent>()
            .Submit(HandlePressableInteract)
            .MatchOriginatorOnly = true;
    }

    private void HandleMouseButton(MouseButtonEvent e, Query<Pressable> pressable, Query<Transform> transform, Query<Extent> extent, Query<Widget> widget) {
        //Logger.DebugLine("PressableInputSystem -> HandleMouseButton");

        switch (e) {
            case MouseButtonEvent mouseButtonEvent:
                /*
                Logger.DebugLine($"----");
                Logger.DebugLine($"Button Event: {mouseButtonEvent}");
                Logger.DebugLine($"Button Global: {transform.Entry.Global}");
                Logger.DebugLine($"Button Local: {transform.Entry.Local}");
                Logger.DebugLine($"Button Bounds: {extent.Entry.GetBounds(transform.Entry.Global)}");
                */

                if (mouseButtonEvent.Button == MouseButton.Left
                 && extent.Entry.GetBounds(transform.Entry.Global).Contains(mouseButtonEvent.Position)
                ) {
                    switch (mouseButtonEvent.State) {
                        case ButtonState.JustPressed:
                            //Logger.DebugLine("was pressed at system");
                            pressable.Entry.Pressed = true;
                            widget.Entry.Builder.RequestEvaluate();
                            Emit(new PressableInteractEvent(pressable));
                            break;
                        case ButtonState.JustReleased:
                            //Logger.DebugLine("was released at system");
                            pressable.Entry.Pressed = false;
                            widget.Entry.Builder.RequestEvaluate();
                            Emit(new PressableInteractEvent(pressable));
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

    private void HandlePressableInteract(PressableInteractEvent ev) {
        BackgroundBorder backgroundBorder = ev.Pressable.Entity.Get<BackgroundBorder>();

        if (ev.Pressable.Pressed) {
            if (backgroundBorder.Background.Drawable != null) {
                backgroundBorder.Background.Drawable.Color = 0x384252FF;
            }
        } else {
            if (backgroundBorder.Background.Drawable != null) {
                backgroundBorder.Background.Drawable.Color = 0x57606FFF;
            }
        }
    }
}
