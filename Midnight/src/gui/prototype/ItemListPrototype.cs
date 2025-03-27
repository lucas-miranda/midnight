
namespace Midnight.GUI;

[PrototypeRegistry(typeof(ItemList), typeof(ItemListBuilder))]
public class ItemListPrototype : Prototype {
    protected override void Build(EntityBuilder builder) {
        builder.Add<Transform>();
        builder.Add<ContentGraphics>();
        builder.Add<Widget>();
        builder.Add<ItemList>();

        builder.Add<GUI.Extent>(new() {
            Padding = new(5),
        });

        var background = builder.Add<DrawableDisplayer>(new() {
            Drawable = new RectangleDrawable() {
                Color = 0x747D8CFF,
                Filled = false,
            },
        });

        builder.With<BackgroundBorder>(new() {
            Background = background,
        });
    }
}
