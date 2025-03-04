namespace Midnight.GUI;

public class PushButtonBuilder : ObjectBuilder {
    public PushButtonBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override Entity Build() {
        return Prototypes.Instantiate<PushButtonPrototype>();
    }

    public override bool Run() {
        var pressable = Result.Get<Pressable>();
        return pressable.Pressed;
    }
}
