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
}
