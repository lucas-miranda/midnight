
namespace Midnight.GUI;

[PrototypeRegistry]
[WidgetPrototypeRegistry(typeof(FrameBuilder))]
public class FramePrototype : Prototype {
    protected override void Build() {
        Add<Transform>();
        Add<ContentGraphics>();
        Add<Widget>();

        Add<Extent>(new() {
            Margin = new(15, 10),
            Padding = new(5),
        });

        var background = Add<RectangleDrawable>(new() {
            Color = 0x747D8CFF,
            Filled = true,
        });

        var border = Add<RectangleDrawable>(new() {
            Color = 0x57606FFF,
            Filled = false,
        });

        Add<BackgroundBorder>(new() {
            Background = background,
            Border = border,
        });
    }
}
