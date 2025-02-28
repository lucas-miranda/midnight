
namespace Midnight.GUI;

public class Button : Container {
    public Button() {
        Background = new() {
            Color = 0x57606FFF,
        };

        Size = new(50, 50);
        Margin = new(15, 10);
        Padding = new(5);
    }

    public bool Pressed { get; private set; }
    public RectangleDrawable Background { get; private set; }

    internal ButtonBuilder Builder { get; set; }

    public override void Input(InputEvent e) {
        base.Input(e);

        switch (e) {
            case MouseButtonEvent mouseButtonEvent:
                //Logger.DebugLine($"----");
                //Logger.DebugLine($"Button Event: {mouseButtonEvent}");
                //Logger.DebugLine($"Button Bounds: {Bounds}");

                if (mouseButtonEvent.Button == MouseButton.Left
                 && Bounds.IsInside(mouseButtonEvent.Position)
                ) {
                    switch (mouseButtonEvent.State) {
                        case ButtonState.JustPressed:
                            Press();
                            break;
                        case ButtonState.JustReleased:
                            Release();
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

    public override void Render(DeltaTime dt) {
        //System.Console.WriteLine($"Draw Button Begin");
        //System.Console.WriteLine($"Button Trans: {Transform}");
        Background.Draw(dt, new DrawParams { Transform = Transform });
        base.Render(dt);
        //System.Console.WriteLine($"Draw Button End");
    }

    public void Press() {
        if (Pressed) {
            return;
        }

        Logger.DebugLine($"GUI Button '{this}' Pressed");
        Pressed = true;
        Builder?.RequestEvaluate();
    }

    public void Release() {
        if (!Pressed) {
            return;
        }

        Logger.DebugLine($"GUI Button '{this}' Released");
        Pressed = false;
        Builder?.RequestEvaluate();
    }

    protected override void SizeChanged() {
        Background.Size = Size;
    }
}
