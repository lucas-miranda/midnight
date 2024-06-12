namespace Midnight.GUI;

public class FrameBuilder : ContainerBuilder {
    public FrameBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override GUI.Frame Build() {
        return new();
    }
}
