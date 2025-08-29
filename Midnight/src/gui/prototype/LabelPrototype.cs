
namespace Midnight.GUI;

[PrototypeRegistry(typeof(Label))]
[WidgetPrototypeRegistry(typeof(LabelBuilder))]
public class LabelPrototype : Prototype {
    protected override void Build() {
        Add<Transform>();
        Add<ContentGraphics>();
        Add<Widget>();

        StringDrawable text = new StringDrawable() {
            Font = AssetManager.Get<Font>("accidental president"),
        };

        Add<Extent>(new() {
            Margin = new(15, 10),
            Padding = new(5),
            Size = text.Size,
        });

        Add(text);

        Add<Label>();
    }
}
