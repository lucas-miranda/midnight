
namespace Midnight.GUI;

[PrototypeRegistry(typeof(ItemList))]
[WidgetPrototypeRegistry(typeof(ItemListBuilder))]
public class ItemListPrototype : Prototype {
    protected override void Build() {
        Add<Transform>();
        Add<ContentGraphics>();
        Add<Widget>();
        Add<ItemList>();

        Add<GUI.Extent>(new() {
            Padding = new(5),
        });

        var background = Add<RectangleDrawable>(new() {
            Color = 0x747D8CFF,
            Filled = false,
        });

        Add<BackgroundBorder>(new() {
            Background = background,
        });
    }
}
