namespace Midnight.GUI;

public class ButtonBuilder : ObjectBuilder {
    public ButtonBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override Button Result {
        get => (Button) base.Result;
    }

    public override GUI.Object Build() {
        return new Button() {
            Builder = this,
        };
    }

    public override bool Run() {
        Button btn = (Button) Result;
        return btn.Pressed;
    }
}
