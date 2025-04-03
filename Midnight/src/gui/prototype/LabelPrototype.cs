
namespace Midnight.GUI;

[PrototypeRegistry(typeof(Label))]
[WidgetPrototypeRegistry(typeof(LabelBuilder))]
public class LabelPrototype : Prototype {
    protected override void Build(EntityBuilder builder) {
        builder.Add<Transform>();
        builder.Add<ContentGraphics>();
        builder.Add<Widget>();

        StringDrawable text = new StringDrawable() {
            Font = AssetManager.Get<Font>("accidental president"),
        };

        builder.Add<Extent>(new() {
            Margin = new(15, 10),
            Padding = new(5),
            Size = text.Size,
        });

        builder.Add<DrawableDisplayer>(new() {
            Drawable = text,
        });

        builder.Add<Label>();
    }
}
