
namespace Midnight.GUI;

[PrototypeRegistry(typeof(FrameBuilder))]
public class FramePrototype : Prototype {
    protected override void Build(EntityBuilder builder) {
        builder.Add<Transform>();
        builder.Add<ContentGraphics>();
        builder.Add<Widget>();

        builder.Add<Extent>(new() {
            Margin = new(15, 10),
            Padding = new(5),
        });

        var background = builder.Add<DrawableDisplayer>(new() {
            Drawable = new RectangleDrawable() {
                Color = 0x747D8CFF,
                Filled = true,
            },
        });

        var border = builder.Add<DrawableDisplayer>(new() {
            Drawable = new RectangleDrawable() {
                Color = 0x57606FFF,
                Filled = false,
            },
        });

        builder.Add<BackgroundBorder>(new() {
            Background = background,
            Border = border,
        });
    }
}
