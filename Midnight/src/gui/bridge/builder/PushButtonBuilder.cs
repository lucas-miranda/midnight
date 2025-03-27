using Midnight.Diagnostics;

namespace Midnight.GUI;

public class PushButtonBuilder : WidgetBuilder {
    public PushButtonBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override Entity Build() {
        Entity buttonEntity = Prototypes.Instantiate<PushButtonPrototype>();
        Widget widget = buttonEntity.Get<Widget>();
        widget.Builder = this;
        return buttonEntity;
    }

    public override bool Run() {
        var pressable = Result.Get<Pressable>();
        return pressable.Pressed;
    }

    public PushButtonBuilder Label(string label) {
        Components components = Result.GetComponents();
        Transform transform = components.Get<Transform>();
        Label childLabel = transform.FindFirstChildWithComponent<Label>();
        DrawableDisplayer displayer = null;

        if (childLabel == null) {
            Entity labelEntity = Prototypes.Instantiate<Label>(Result);
            displayer = labelEntity.Get<DrawableDisplayer>();
        } else {
            displayer = childLabel.Entity.Get<DrawableDisplayer>();
        }

        Assert.NotNull(displayer);
        Assert.Is<StringDrawable>(displayer.Drawable);

        // set label text value
        StringDrawable text = (StringDrawable) displayer.Drawable;
        text.Value = label;

        return this;
    }
}
