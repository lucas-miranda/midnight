namespace Midnight.GUI;

public class FrameBuilder : ContainerBuilder {
    public FrameBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override Entity Build() {
        return Prototypes.Instantiate<FramePrototype>();
    }
}
