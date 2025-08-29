
namespace Midnight.GUI;

[PrototypeRegistry]
[WidgetPrototypeRegistry(typeof(PushButtonBuilder))]
public class PushButtonPrototype : Prototype {
    protected override void Build() {
        Add<Transform>();
        Add<ContentGraphics>();
        Add<Widget>();

        Add<GUI.Extent>(new() {
            Margin = new(15, 10),
            Padding = new(5),
        });

        var background = Add<RectangleDrawable>(new() {
            Color = 0x57606FFF,
            Filled = true,
        });

        With<BackgroundBorder>(new() {
            Background = background,
        });

        With<Pressable>();
    }
}
