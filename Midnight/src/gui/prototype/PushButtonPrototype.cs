
namespace Midnight.GUI;

public class PushButtonPrototype : Prototype {
    protected override void Build(EntityBuilder builder) {
        builder.Add<Transform>();
        builder.Add<ContentGraphics>();

        builder.Add<GUI.Extent>(new() {
            Margin = new(15, 10),
            Padding = new(5),
        });

        var background = builder.Add<DrawableDisplayer>(new() {
            Drawable = new RectangleDrawable() {
                Color = 0x57606FFF,
                Filled = true,
            },
        });

        builder.With<BackgroundBorder>(new() {
            Background = background,
        });

        builder.With<Pressable>();
    }
}
