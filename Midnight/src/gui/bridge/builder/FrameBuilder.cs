namespace Midnight.GUI;

public class FrameBuilder : ContainerBuilder {
    public FrameBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override Entity Build() {
        Entity frameEntity = Prototypes.Instantiate<FramePrototype>();
        Widget widget = frameEntity.Get<Widget>();
        widget.Builder = this;
        return frameEntity;
    }
}
