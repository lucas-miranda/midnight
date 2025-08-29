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
        Transform transform = Result.Get<Transform>();
        Label childLabel = transform.FindFirstChildWithComponent<Label>();
        Drawable drawable = null;

        if (childLabel == null) {
            Entity labelEntity = Prototypes.Instantiate<Label>(Result);
            drawable = labelEntity.Get<Drawable>();
        } else {
            drawable = childLabel.Entity.Get<Drawable>();
        }

        Assert.NotNull(drawable);
        Assert.Is<StringDrawable>(drawable);

        // set label text value
        StringDrawable text = (StringDrawable) drawable;
        text.Value = label;

        return this;
    }
}
